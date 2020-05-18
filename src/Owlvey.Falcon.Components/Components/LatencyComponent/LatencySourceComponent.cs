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
    public class LatencySourceComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        public LatencySourceComponent(FalconDbContext dbContext, IDateTimeGateway dataTimeGateway,
            IMapper mapper, IUserIdentityGateway identityService,
            ConfigurationComponent configuration) : base(dataTimeGateway, mapper, identityService, configuration)
        {
            this._dbContext = dbContext;
        }

        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SourceEntity, LatencySourceGetRp>()
                .ForMember(c => c.Features, opt => opt.Ignore());

            cfg.CreateMap<SourceItemEntity, LatencySourceItemGetRp>();                
        }

        public async Task<LatencySourceGetRp> GetByIdWithDetail(int id, DatePeriodValue period) {

            var entity = await this._dbContext.Sources
              .Include(c => c.Indicators)
              .ThenInclude(c => c.Feature)
              .SingleOrDefaultAsync(c => c.Id == id);

            var result = this._mapper.Map<LatencySourceGetRp>(entity);
            if (entity != null)
            {
                var sourceItems = await this._dbContext.GetSourceItems(entity.Id.Value, period.Start, period.End);

                var ids = sourceItems.Select(c => c.Id).ToList();                

                entity.SourceItems = sourceItems;
                var measure = entity.Measure();
                result.Latency = measure.Value;                
                result.Features = entity.FeaturesToDictionary();
            }
            return result;
        }

        public async Task<LatencySourceGetRp> GetById(int id)
        {

            var entity = await this._dbContext.Sources
              .Include(c => c.Indicators)
              .ThenInclude(c => c.Feature)
              .SingleOrDefaultAsync(c => c.Id == id);

            var result = this._mapper.Map<LatencySourceGetRp>(entity);
            if (entity != null)
            {   
                result.Features = entity.FeaturesToDictionary();
            }
            return result;
        }
        public async Task<IEnumerable<LatencySourceItemGetRp>> GetItems(int sourceId, DatePeriodValue period)
        {
            var entities = await this._dbContext.GetSourceItems(sourceId, period.Start, period.End);
            var result = this._mapper.Map<IEnumerable<LatencySourceItemGetRp>>(entities);
            return result;
        }

        public async Task<IEnumerable<SourceItemBaseRp>> CreateLatency(LatencySourceItemPostRp model)
        {
            var createdBy = this._identityService.GetIdentity();
            var on = this._datetimeGateway.GetCurrentDateTime();
            var source = await this._dbContext.Sources.SingleAsync(c => c.Id == model.SourceId);

            var range = SourceEntity.Factory.CreateLatencyFromRange(source, model.Start, model.End, model.Latency, on, createdBy);

            foreach (var item in range)
            {
                this._dbContext.SourcesItems.Add(item);
            }

            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<IEnumerable<SourceItemBaseRp>>(range);
        }

        public async Task<BaseComponentResultRp> Update(int sourceId, LatencySourcePutRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();

            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            var entity = await this._dbContext.Sources.Where(c => c.Id == sourceId).SingleAsync();

            entity.Update(model.Name, model.Avatar, model.GoodDefinition, model.TotalDefinition,
                this._datetimeGateway.GetCurrentDateTime(), createdBy, null, model.Description, model.Percentile);

            this._dbContext.Update(entity);

            await this._dbContext.SaveChangesAsync();

            result.AddResult("Id", entity.Id);

            return result;
        }

    }
}
