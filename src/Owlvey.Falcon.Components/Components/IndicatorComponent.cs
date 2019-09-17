using System;
using System.Threading.Tasks;
using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core;

namespace Owlvey.Falcon.Components
{
    public class IndicatorComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;

        public IndicatorComponent(FalconDbContext dbContext, IDateTimeGateway dataTimeGateway, IMapper mapper, IUserIdentityGateway identityService) : base(dataTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
        }

        public async Task<BaseComponentResultRp> Delete(int indicatorId) {
            var result = new BaseComponentResultRp();
            var entity = await this._dbContext.Indicators.Where(c => c.Id == indicatorId).SingleOrDefaultAsync();
            if (entity != null) {
                this._dbContext.Indicators.Remove(entity);
                await this._dbContext.SaveChangesAsync();
            }
            return result;
        }

        public async Task<IndicatorGetListRp>  Create(int customerId, string product, string source, string feature)
        {
            var createdBy = this._identityService.GetIdentity();
            var productEntity = await this._dbContext.Products.Where(c => c.CustomerId == customerId && c.Name == product).SingleAsync();
            var sourceEntity = await this._dbContext.Sources.Where(c => c.ProductId == productEntity.Id && c.Name == source).SingleAsync();
            var featureEntity = await this._dbContext.Features.Where(c => c.ProductId == productEntity.Id && c.Name == feature).SingleAsync();

            var sli = await this._dbContext.Indicators.Where(c => 
                            c.FeatureId == featureEntity.Id && c.SourceId == sourceEntity.Id).SingleOrDefaultAsync();
            if (sli == null) {
                sli = IndicatorEntity.Factory.Create(featureEntity, sourceEntity,
                    this._datetimeGateway.GetCurrentDateTime(), createdBy);
                this._dbContext.Indicators.Add(sli);
                await this._dbContext.SaveChangesAsync();
            }
            return this._mapper.Map<IndicatorGetListRp>(sli);
        }

        public async Task<IndicatorGetListRp> Create(int featureId, int sourceId) {
            var createdBy = this._identityService.GetIdentity();
            var sli = await this._dbContext.Indicators.Where(c => c.FeatureId == featureId && c.SourceId == sourceId).SingleOrDefaultAsync();
            if (sli == null) {
                var feature = await this._dbContext.Features.Where(c => c.Id == featureId).SingleAsync();
                var source = await this._dbContext.Sources.Where(c => c.Id == sourceId).SingleAsync();
                sli = IndicatorEntity.Factory.Create(feature, source, this._datetimeGateway.GetCurrentDateTime(), createdBy);
                this._dbContext.Indicators.Add(sli);
                await this._dbContext.SaveChangesAsync();
            }
            return this._mapper.Map<IndicatorGetListRp>(sli);
        }

        public async Task<BaseComponentResultRp> Create(IndicatorPostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();
            var source = await this._dbContext.Sources.SingleAsync(c => c.Id == model.SourceId);
            var feature = await this._dbContext.Features.Include(c=>c.Indicators).SingleAsync(c => c.Id == model.FeatureId);
            
            // Validate if the resource exists.
            if (feature.Indicators.Any(c => c.SourceId == source.Id))
            {
                result.AddConflict($"The Resource {model.FeatureId} has already been registered.");
                return result;
            }

            var entity = IndicatorEntity.Factory.Create(feature, source, this._datetimeGateway.GetCurrentDateTime(), createdBy);
            await this._dbContext.Indicators.AddAsync(entity);
            await this._dbContext.SaveChangesAsync();

            result.AddResult("Id", entity.Id);

            return result;
        }


        public async Task<IEnumerable<SourceGetListRp>> GetSourcesComplement(int featureId)
        {
            IEnumerable<SourceGetListRp> result = new List<SourceGetListRp>();
            var feature = await this._dbContext.Features.SingleAsync(c => c.Id == featureId);
            
            var sources = await this._dbContext.Sources.Where(c=>c.ProductId == feature.ProductId).ToListAsync();
            
            var existing = await this._dbContext.Indicators.Include(c => c.Source)
                .Where(c => c.FeatureId == featureId).Select(c => c.Source).ToListAsync();
            result = this._mapper.Map<IEnumerable<SourceGetListRp>>(sources.Except(existing, new SourceEntityComparer()));            
            return result;
        }

        public async Task<IEnumerable<IndicatorGetListRp>> GetByFeature(int featureId)
        {
            var entity = await this._dbContext.Indicators.Include(c=>c.Feature).Include(c=>c.Source).Where(c => c.Feature.Id == featureId).ToListAsync();
            return this._mapper.Map<IEnumerable<IndicatorGetListRp>>(entity);
        }

        private async Task<decimal> GetAvailabilityByIndicator(IndicatorEntity entity, DateTime start, DateTime end)
        {
            var sourceItems = await this._dbContext.GetSourceItems(entity.Source.Id.Value, start, end);
            entity.Source.SourceItems = sourceItems;
            var agg = new IndicatorDateAvailabilityAggregate(entity);
            return agg.MeasureAvailability();
        }
        
        public async Task<IEnumerable<IndicatorGetListRp>> GetByFeatureWithAvailability(int featureId, DateTime start, DateTime end)
        {
            var entities = await this._dbContext.Indicators.Include(c => c.Feature).Include(c => c.Source).Where(c => c.Feature.Id == featureId).ToListAsync();
            var result = new List<IndicatorGetListRp>();
            foreach (var item in entities)
            {                
                var tmp = this._mapper.Map<IndicatorGetListRp>(item);
                tmp.Availability = await this.GetAvailabilityByIndicator(item, start, end);
                result.Add(tmp);
            }
            return result;
        }

        public async Task<IndicatorGetRp> GetById(int id)
        {
            var entity = await this._dbContext.Indicators.Include(c=>c.Feature).Include(c=> c.Source).SingleOrDefaultAsync(c => c.Id == id);

            return this._mapper.Map<IndicatorGetRp>(entity);
        }
        #region reports
        public async Task<SeriesGetRp> GetDailySeriesById(int indicatorId, DateTime start, DateTime end)
        {
            var indicator = await this._dbContext.Indicators.Include(c=>c.Source).SingleAsync(c => c.Id == indicatorId);
            var sourceItems = await this._dbContext.SourcesItems.Where(c => c.SourceId == indicator.Source.Id && c.Start >= start && c.End <= end).ToListAsync();

            indicator.Source.SourceItems = sourceItems;
            
            var result = new SeriesGetRp
            {
                Start = start,
                End = end,
                Avatar = indicator.Avatar,
                Name = String.Format($"{indicator.Feature.Name}-{indicator.Source.Name}")
            };

            //TODO: refactoring

            var aggregator = new IndicatorAvailabilityAggregator(indicator, start, end);

            var (_, items) = aggregator.MeasureAvailability();
            
            foreach (var item in items)
            {
                result.Items.Add(this._mapper.Map<SeriesItemGetRp>(item));
            }            

            return result;
        }
        #endregion
    }
}
