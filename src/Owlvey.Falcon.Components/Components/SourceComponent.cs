using System;
using System.Threading.Tasks;
using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core;
using Owlvey.Falcon.Repositories.Sources;

namespace Owlvey.Falcon.Components
{
    public class SourceComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;

        public SourceComponent(FalconDbContext dbContext, IDateTimeGateway dataTimeGateway,
            IMapper mapper, IUserIdentityGateway identityService) : base(dataTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
        }

        public async Task<BaseComponentResultRp> Update(int sourceId, SourcePutRp model) {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();
            
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            var entity = await this._dbContext.Sources.Where(c => c.Id == sourceId).SingleAsync();

            entity.Update(model.Name, model.Avatar, model.GoodDefinition, model.TotalDefinition,
                this._datetimeGateway.GetCurrentDateTime(), createdBy, null, model.Description);
                       
            this._dbContext.Update(entity);

            await this._dbContext.SaveChangesAsync();

            result.AddResult("Id", entity.Id);

            return result;
        }


        public async Task<SourceGetListRp> CreateOrUpdate(CustomerEntity customer, string product, string name, string tags,
            string avatar, string good, string total, string description, string kind, string group)
        {

            group = string.IsNullOrWhiteSpace(group)? "Availability" : group;

            var createdBy = this._identityService.GetIdentity();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;            
            var entity = await this._dbContext.Sources.Where(c =>c.Product.CustomerId == customer.Id && c.Product.Name == product && c.Name == name).SingleOrDefaultAsync();
            if (entity == null)
            {
                var productEntity = await this._dbContext.Products.Where(c => c.CustomerId == customer.Id && c.Name == product).SingleAsync();
                entity = SourceEntity.Factory.Create(productEntity, name, this._datetimeGateway.GetCurrentDateTime(), 
                    createdBy, (SourceKindEnum)Enum.Parse(typeof(SourceKindEnum), kind),
                    (SourceGroupEnum)Enum.Parse(typeof(SourceGroupEnum), group));
            }
            entity.Update(name, avatar, good, total, this._datetimeGateway.GetCurrentDateTime(), createdBy, tags, description);
            this._dbContext.Sources.Update(entity);
            await this._dbContext.SaveChangesAsync();            
            return this._mapper.Map<SourceGetListRp>(entity);
        }


        public async Task<SourceGetListRp> Create(SourcePostRp model)
        {            
            var createdBy = this._identityService.GetIdentity();

            var entity = await this._dbContext.GetSource(model.ProductId, model.Name);

            if (entity == null) {
                var product = await this._dbContext.Products.SingleAsync(c => c.Id == model.ProductId);
                entity = SourceEntity.Factory.Create(product, model.Name, 
                    this._datetimeGateway.GetCurrentDateTime(), createdBy, 
                    model.Kind, model.Group);
                await this._dbContext.AddAsync(entity);
                await this._dbContext.SaveChangesAsync();
            }

            return this._mapper.Map<SourceGetListRp>(entity);
        }

        public async Task Delete(int sourceId) {
            var createdBy = this._identityService.GetIdentity();
            var entity = await this._dbContext.Sources.Where(c => c.Id == sourceId).SingleOrDefaultAsync();
            if (entity != null)
            {                   
                foreach (var indicator in entity.Indicators)
                {
                    this._dbContext.Indicators.Remove(indicator);
                }

                await this._dbContext.SaveChangesAsync();

                this._dbContext.Sources.Remove(entity);

                await this._dbContext.SaveChangesAsync();
            }            
        }
        public async Task<SourceGetRp> GetByName(int productId, string name)
        {
            var entity = await this._dbContext.Sources.SingleOrDefaultAsync(c => c.Product.Id == productId && c.Name == name);
            return this._mapper.Map<SourceGetRp>(entity);
        }

        public async Task<SourceGetRp> GetById(int id)
        {
            var entity = await this._dbContext.Sources.SingleOrDefaultAsync(c => c.Id == id);
            return this._mapper.Map<SourceGetRp>(entity);
        }
        public async Task<SourceGetRp> GetByIdWithAvailability(int id, DateTime start, DateTime end)
        {
            var entity = await this._dbContext.Sources.SingleOrDefaultAsync(c => c.Id == id);
            var result = this._mapper.Map<SourceGetRp>(entity);
            if (entity!= null) {
                var (ava, total, good) = await GetAvailabilityBySource(entity, start, end);
                result.Availability = ava;
                result.Total = total;
                result.Good = good;
            }            
            return result;
        }

        private async Task<(decimal, int, int)> GetAvailabilityBySource(SourceEntity entity, DateTime start, DateTime end) {
            var sourceItems = await this._dbContext.GetSourceItems(entity.Id.Value, start, end);
            entity.SourceItems = sourceItems;
            var agg = new SourceAvailabilityAggregate(entity);
            return agg.MeasureAvailability();
        }

        public async Task<IEnumerable<SourceGetListRp>> GetByProductId(int productId)
        {
            var entities = await this._dbContext.Sources.Where(c => c.Product.Id == productId).ToListAsync();
            return this._mapper.Map<IEnumerable<SourceGetListRp>>(entities);
        }
        public async Task<IEnumerable<SourceGetListRp>> GetByProductIdWithAvailability(int productId, DateTime start, DateTime end)
        {
            var entities = await this._dbContext.Sources.Include(c=>c.Indicators).Where(c => c.Product.Id == productId).ToListAsync();            
            var sourceItems = await this._dbContext.GetSourceItems(start, end);

            var result = new List<SourceGetListRp>();
            foreach (var source in entities)
            {                
                source.SourceItems = sourceItems.Where(c => c.SourceId == source.Id).ToList();
                var agg = new SourceAvailabilityAggregate(source);
                var (ava, total, good)= agg.MeasureAvailability();

                var tmp = this._mapper.Map<SourceGetListRp>(source);
                tmp.References = source.Indicators.Count();
                tmp.Availability = ava;
                tmp.Total = total;
                tmp.Good = good; 
                result.Add(tmp);
            }
            return result.OrderBy(c=>c.Availability).ToList();
        }

        public async Task<IEnumerable<SourceGetListRp>> GetByIndicatorId(int indicatorId)
        {
            var entities = await this._dbContext.Indicators.Include(c=>c.Source).Where(c=>c.Id == indicatorId).ToListAsync();
            var targets = entities.Select(c => c.Source).ToList();
            return this._mapper.Map<IEnumerable<SourceGetListRp>>(targets);
        }

        
        public async Task<SeriesGetRp> GetDailySeriesById(int sourceId, DateTime start, DateTime end) {                                   
            var source = await this._dbContext.Sources.SingleAsync(c => c.Id == sourceId);
            var sourceItems = await this._dbContext.GetSourceItems(sourceId, start, end);
            source.SourceItems = sourceItems;
            var result = new SeriesGetRp
            {
                Start = start,
                End = end,
                Name = source.Name,
                Avatar = source.Avatar
            };

            var aggregator = new SourceDailyAvailabilityAggregate(source, start, end);
            var (_, items) = aggregator.MeasureAvailability();                        

            foreach (var item in items)
            {
                result.Items.Add(this._mapper.Map<SeriesItemGetRp>(item));                
            }            
            
            return result;
        }
    }
}
