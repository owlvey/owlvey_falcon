using System;
using System.Threading.Tasks;
using AutoMapper;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Core.Entities;
using System.Collections.Generic;
using Owlvey.Falcon.Core;
using Owlvey.Falcon.Core.Values;
using Owlvey.Falcon.Components.Models;
using Owlvey.Falcon.Repositories.Sources;
using Owlvey.Falcon.Core.Aggregates;

namespace Owlvey.Falcon.Components
{
    public class SourceItemComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;

        public SourceItemComponent(FalconDbContext dbContext, 
            IDateTimeGateway dataTimeGateway, 
            IMapper mapper, 
            IUserIdentityGateway identityGateway,
            ConfigurationComponent configuration) : base(dataTimeGateway, mapper, identityGateway, configuration)
        {
            this._dbContext = dbContext;            
        }


        private IEnumerable<SourceItemEntity> CreateFromPostRp(SourceEntity source, SourceItemPostRp model, DateTime on, string createdBy) {
            if (model is SourceItemAvailabilityPostRp ava)
            {
                return SourceEntity.Factory.CreateItemsFromRange(source, model.Start,
                    model.End, ava.Good, ava.Total, ava.Measure, on, createdBy,
                    SourceGroupEnum.Availability);
            }
            else if (model is SourceItemLatencyPostRp latency)
            {
                return SourceEntity.Factory.CreateItemsFromRangeByMeasure(source, model.Start,
                    model.End, latency.Measure, on, createdBy,
                    SourceGroupEnum.Latency);
            }
            else if (model is SourceItemExperiencePostRp experience)
            {
                return SourceEntity.Factory.CreateItemsFromRange(source, model.Start,
                    model.End, experience.Good, experience.Total, 
                    experience.Measure, on, createdBy,
                    SourceGroupEnum.Experience);
            }
            else {
                throw new ApplicationException("type is not valid");
            }
        }
        public async Task<IEnumerable<SourceItemBaseRp>> CreateLatencyItem(SourceItemLatencyPostRp model)
        {
            var createdBy = this._identityGateway.GetIdentity();
            var on = this._datetimeGateway.GetCurrentDateTime();
            var source = await this._dbContext.Sources.SingleAsync(c => c.Id == model.SourceId);

            var range = SourceEntity.Factory.CreateItemsFromRangeByMeasure(source, model.Start, 
                model.End, model.Measure, on, createdBy,
                SourceGroupEnum.Latency);

            foreach (var item in range)
            {
                this._dbContext.SourcesItems.Add(item);
            }

            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<IEnumerable<SourceItemBaseRp>>(range);
        }

        public async Task<IEnumerable<SourceItemBaseRp>> CreateExperienceItem(SourceItemExperiencePostRp model)
        {
            var createdBy = this._identityGateway.GetIdentity();
            var on = this._datetimeGateway.GetCurrentDateTime();
            var source = await this._dbContext.Sources.SingleAsync(c => c.Id == model.SourceId);

            var range = SourceEntity.Factory.CreateItemsFromRange(source, model.Start, 
                model.End, 
                model.Good, 
                model.Total, 
                model.Measure, 
                on, createdBy,  SourceGroupEnum.Experience );

            foreach (var item in range)
            {
                this._dbContext.SourcesItems.Add(item);
            }

            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<IEnumerable<SourceItemBaseRp>>(range);

        }   

        public async Task<ScalabilitySourceGetRp> GetScalability(int sourceId, DatePeriodValue period) {            
            var source = await this._dbContext.GetSourceWithItems(sourceId, period);
            
            var model = new ScalabilitySourceGetRp
            {
                Period = period,
            };
            var agg = new ScalabilityMeasureAggregate(source.SourceItems);
            (model.DailyCorrelation, model.DailyTotal, model.DailyBad, model.DailyIntercept, 
                model.DailySlope, model.DailyR2, model.DailyInteractions) = agg.Measure();
            
            return model;
        }
        public async Task<IEnumerable<SourceItemBaseRp>> CreateAvailabilityItem(SourceItemAvailabilityPostRp model)
        {
            var createdBy = this._identityGateway.GetIdentity();
            var on = this._datetimeGateway.GetCurrentDateTime();
            var source = await this._dbContext.Sources.SingleAsync(c => c.Id == model.SourceId);

            var range = SourceEntity.Factory.CreateItemsFromRange(source, 
                model.Start, 
                model.End, 
                model.Good, 
                model.Total, 
                model.Measure,
                on, 
                createdBy, 
                SourceGroupEnum.Availability);

            foreach (var item in range)
            {
                this._dbContext.SourcesItems.Add(item);
            }

            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<IEnumerable<SourceItemBaseRp>>(range);
        }

        public async Task<IEnumerable<LatencySourceItemGetRp>> GetLatencyItems(int sourceId, DatePeriodValue period)
        {
            var entities = await this._dbContext.GetSourceItems(sourceId, SourceGroupEnum.Latency, period.Start, period.End);
            var result = this._mapper.Map<IEnumerable<LatencySourceItemGetRp>>(entities);
            return result;
        }
        public async Task<IEnumerable<ExperienceSourceItemGetRp>> GetExperienceItems(int sourceId, DatePeriodValue period)
        {
            var entities = await this._dbContext.GetSourceItems(sourceId, SourceGroupEnum.Experience, period.Start, period.End);
            var result = this._mapper.Map<IEnumerable<ExperienceSourceItemGetRp>>(entities);
            return result;
        }
        public async Task<IEnumerable<AvailabilitySourceItemGetRp>> GetAvailabilityItems(int sourceId, DatePeriodValue period)
        {
            IEnumerable<SourceItemEntity> entities = await this._dbContext.GetSourceItems(sourceId, SourceGroupEnum.Availability, period.Start, period.End);
            var result = this._mapper.Map<IEnumerable<AvailabilitySourceItemGetRp>>(entities);
            return result;
        }

       
        public async Task BulkInsert(IEnumerable<SourceItemEntity> sourceItems) {
            
            int period = 0;                        
            foreach (var model in sourceItems)
            {   
                this._dbContext.SourcesItems.Add(model);
                period += 1;                
                if (period > 1000) {                    
                    await this._dbContext.SaveChangesAsync();
                    period = 0;
                }
            }
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<SourceItemGetListRp> Update(int sourceItemId,
            int total, int good, DateTime target) {

            var entity = await this._dbContext.SourcesItems.Where(c => c.Id == sourceItemId).SingleAsync();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;                        
            entity.Update(total, good, target);                            
            this._dbContext.SourcesItems.Update(entity);
            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<SourceItemGetListRp>(entity);            
        }
        public async Task<SourceItemGetListRp> Update(int sourceItemId, decimal proportion, DateTime target)
        {
            var entity = await this._dbContext.SourcesItems.Where(c => c.Id == sourceItemId).SingleAsync();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            entity.Update(proportion, target);
            this._dbContext.SourcesItems.Update(entity);
            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<SourceItemGetListRp>(entity);
        }
        public async Task<BaseComponentResultRp> Delete(int id)
        {
            var result = new BaseComponentResultRp();
            
            var source = await this._dbContext.SourcesItems.SingleAsync(c => c.Id == id);

            this._dbContext.SourcesItems.Remove(source);

            await this._dbContext.SaveChangesAsync();
            
            return result;
        }

        public async Task<BaseComponentResultRp> DeleteSource(int sourceId)
        {
            var result = new BaseComponentResultRp();

            var items = await this._dbContext.SourcesItems.Where(c=> c.SourceId == sourceId).ToListAsync();

            foreach (var item in items)
            {
                this._dbContext.SourcesItems.Remove(item);
            }            

            await this._dbContext.SaveChangesAsync();

            return result;
        }


        public async Task<IEnumerable<SourceItemGetListRp>> GetBySource(int sourceId)
        {
            var entities = await this._dbContext.SourcesItems.Where(c => c.SourceId == sourceId).ToListAsync();

            entities = entities.OrderBy(c => c.Target).ToList();

            return this._mapper.Map<IEnumerable<SourceItemGetListRp>>(entities);
        }
                
        public async Task<IEnumerable<SourceItemGetListRp>> GetAll()
        {
            var entity = await this._dbContext.SourcesItems.ToListAsync();
            return this._mapper.Map<IEnumerable<SourceItemGetListRp>>(entity);
        }
        
        public async Task<SourceItemGetListRp> GetById(int id)
        {
            var entity = await this._dbContext.SourcesItems                
                .Where(c => c.Id == id).SingleAsync();
            return this._mapper.Map<SourceItemGetListRp>(entity);
        }

        public async Task<IEnumerable<SourceItemGetListRp>> GetBySourceIdAndDateRange(int sourceId, 
            DateTime start, DateTime end)
        {
            var entity = await this._dbContext.GetSourceItems(sourceId, start, end);
            return this._mapper.Map<IEnumerable<SourceItemGetListRp>>(entity);
        }


    }
}
