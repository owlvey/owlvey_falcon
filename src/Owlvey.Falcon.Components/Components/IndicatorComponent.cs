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

namespace Owlvey.Falcon.Components
{
    public class IndicatorComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;

        public IndicatorComponent(FalconDbContext dbContext, IDateTimeGateway dataTimeGateway, IMapper mapper, IUserIdentityGateway identityService) : base(dataTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
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

        public async Task<IEnumerable<IndicatorGetRp>> GetByFeature(int featureId)
        {
            var entity = await this._dbContext.Indicators.Where(c => c.Feature.Id == featureId).ToListAsync();
            return this._mapper.Map<IEnumerable<IndicatorGetRp>>(entity);
        }

        public async Task<IndicatorGetRp> GetById(int id)
        {
            var entity = await this._dbContext.Indicators.SingleOrDefaultAsync(c => c.Id == id);
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
