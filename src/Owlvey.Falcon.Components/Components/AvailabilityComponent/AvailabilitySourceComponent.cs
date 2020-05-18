using AutoMapper;
using Owlvey.Falcon.Core.Values;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Components.Models;

namespace Owlvey.Falcon.Components
{
    public class AvailabilitySourceComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        public AvailabilitySourceComponent(FalconDbContext dbContext, IDateTimeGateway dataTimeGateway,
            IMapper mapper, IUserIdentityGateway identityService,
            ConfigurationComponent configuration) : base(dataTimeGateway, mapper, identityService, configuration)
        {
            this._dbContext = dbContext;
        }

        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {

        }
        public async Task<ProportionSourceGetRp> GetProprotionById(int id, DatePeriodValue period)
        {
            var entity = await this._dbContext.Sources
              .Include(c => c.Indicators)
              .ThenInclude(c => c.Feature)
              .SingleOrDefaultAsync(c => c.Id == id);

            var result = this._mapper.Map<ProportionSourceGetRp>(entity);
            if (entity != null)
            {
                var sourceItems = await this._dbContext.GetSourceItems(entity.Id.Value, period.Start, period.End);
                var ids = sourceItems.Select(c => c.Id).ToList();
                entity.SourceItems = sourceItems;
                var measure = entity.Measure();
                result.Proportion = measure.Value;
                result.Features = entity.FeaturesToDictionary();
            }
            return result;
        }

        public async Task<InteractionSourceGetRp> GetInteractionById(int id, DatePeriodValue period)
        {
            var entity = await this._dbContext.Sources
              .Include(c => c.Indicators)
              .ThenInclude(c => c.Feature)
              .SingleOrDefaultAsync(c => c.Id == id);

            var result = this._mapper.Map<InteractionSourceGetRp>(entity);
            if (entity != null)
            {
                var sourceItems = await this._dbContext.GetSourceItems(entity.Id.Value, period.Start, period.End);
                var ids = sourceItems.Select(c => c.Id).ToList();
                entity.SourceItems = sourceItems;
                var measure = (InteractionMeasureValue)entity.Measure();
                result.Proportion = measure.Value;
                result.Good = measure.Good;
                result.Total = measure.Total;
                result.Features = entity.FeaturesToDictionary();
            }
            return result;
        }

        public async Task<IEnumerable<InteractiveSourceItemGetRp>> GetInteractionItems(int sourceId, DatePeriodValue period) {
            var entities = await this._dbContext.GetSourceItems(sourceId, period.Start, period.End);
            var result = this._mapper.Map<IEnumerable<InteractiveSourceItemGetRp>>(entities);
            return result;
        }

        public async Task<IEnumerable<ProportionSourceItemGetRp>> GetProportionItems(int sourceId, DatePeriodValue period)
        {
            var entities = await this._dbContext.GetSourceItems(sourceId, period.Start, period.End);
            var result = this._mapper.Map<IEnumerable<ProportionSourceItemGetRp>>(entities);
            return result;
        }

        public async Task<IEnumerable<SourceItemBaseRp>> CreateInteraction(SourceItemInteractionPostRp model)
        {
            var createdBy = this._identityService.GetIdentity();
            var on = this._datetimeGateway.GetCurrentDateTime();
            var source = await this._dbContext.Sources.SingleAsync(c => c.Id == model.SourceId);

            var range = SourceEntity.Factory.CreateInteractionsFromRange(source, model.Start, model.End, model.Good, model.Total, on, createdBy);

            foreach (var item in range)
            {
                this._dbContext.SourcesItems.Add(item);
            }

            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<IEnumerable<SourceItemBaseRp>>(range);
        }

        public async Task<IEnumerable<SourceItemBaseRp>> CreateProportion(SourceItemProportionPostRp model)
        {
            var createdBy = this._identityService.GetIdentity();
            var on = this._datetimeGateway.GetCurrentDateTime();
            var source = await this._dbContext.Sources.SingleAsync(c => c.Id == model.SourceId);

            var range = SourceEntity.Factory.CreateProportionFromRange(source, model.Start, model.End, model.Proportion, on, createdBy);

            foreach (var item in range)
            {
                this._dbContext.SourcesItems.Add(item);
            }

            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<IEnumerable<SourceItemBaseRp>>(range);
        }

        public async Task<BaseComponentResultRp> Update(int sourceId, SourcePutRp model)
        {
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

    }
}
