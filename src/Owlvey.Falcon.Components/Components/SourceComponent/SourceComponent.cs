using System;
using System.Linq;
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
using Polly;
using Owlvey.Falcon.Core.Values;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Owlvey.Falcon.Components.Models;

namespace Owlvey.Falcon.Components
{
    public class SourceComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;

        public SourceComponent(FalconDbContext dbContext, IDateTimeGateway dataTimeGateway,
            IMapper mapper, IUserIdentityGateway identityService,
            ConfigurationComponent configuration) : base(dataTimeGateway, mapper, identityService, configuration)
        {
            this._dbContext = dbContext;
        }

        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
           
            cfg.CreateMap<SourceItemEntity, SourceItemBaseRp>();
            cfg.CreateMap<SourceItemEntity, ProportionSourceItemGetRp>();





        }

      

        public async Task<SourceGetListRp> CreateOrUpdate(CustomerEntity customer, string product, string name, string tags,
            string avatar, string good, string total, string description, string kind, string group, decimal percentile)
        {   
            group = string.IsNullOrWhiteSpace(group)? "Availability" : group;

            var parseGroup = (SourceGroupEnum)Enum.Parse(typeof(SourceGroupEnum), group);

            var createdBy = this._identityService.GetIdentity();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            var entity = await this._dbContext.Sources.Where(c => c.Product.CustomerId == customer.Id && c.Product.Name == product && c.Name == name).SingleOrDefaultAsync();

            if (entity == null)
            {
                var productEntity = await this._dbContext.Products.Where(c => c.CustomerId == customer.Id && c.Name == product).SingleAsync();
                entity = SourceEntity.Factory.Create(productEntity, name, this._datetimeGateway.GetCurrentDateTime(), createdBy);                
            }
            entity.Update(name, avatar, good, total, this._datetimeGateway.GetCurrentDateTime(),
                    createdBy, tags, description);
            this._dbContext.Sources.Update(entity);
            await this._dbContext.SaveChangesAsync();            
            return this._mapper.Map<SourceGetListRp>(entity);
        }


        public async Task<SourceGetListRp> Create(SourcePostRp model)
        {   
            var createdBy = this._identityService.GetIdentity();

            var product = await this._dbContext.Products.SingleAsync(c => c.Id == model.ProductId);

            var retryPolicy = Policy.Handle<DbUpdateException>()
                .WaitAndRetryAsync(this._configuration.DefaultRetryAttempts,
                i => this._configuration.DefaultPauseBetweenFails);

            return await retryPolicy.ExecuteAsync(async () =>
            {
                var entity = await this._dbContext.GetSource(model.ProductId, model.Name);

                if (entity == null)
                {                    
                    entity = SourceEntity.Factory.Create(product, model.Name,
                        this._datetimeGateway.GetCurrentDateTime(), createdBy);
                    await this._dbContext.AddAsync(entity);
                    await this._dbContext.SaveChangesAsync();
                }

                return this._mapper.Map<SourceGetListRp>(entity);
            });


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
            var entity = await this._dbContext.Sources                
                .SingleOrDefaultAsync(c => c.Id == id);
            return this._mapper.Map<SourceGetRp>(entity);
        }
        public async Task<SourceAnchorRp> GetAnchor(int id) {
            var entity = await this._dbContext.Sources
                .SingleOrDefaultAsync(c => c.Id == id);
                        
            var item = this._dbContext.SourcesItems
                .Where(c => c.SourceId == id)
                .OrderByDescending(c => c.Target)
                .Take(1).ToList();

            var result = new SourceAnchorRp();
            result.Id = entity.Id.Value;
            result.Name = entity.Name;

            if (item.Count() == 0){
                result.Target = new DateTime(this._datetimeGateway.GetCurrentDateTime().Year, 1, 1);
            }
            else {
                result.Target = item.ElementAt(0).Target;
            }
            return result;
        }
        public async Task<SourceGetRp> GetByIdWithAvailability(int id, DateTime start, DateTime end)
        {
            var entity = await this._dbContext.Sources
                .Include(c=>c.Indicators)
                .ThenInclude(c=>c.Feature)
                .SingleOrDefaultAsync(c => c.Id == id);

            

            var result = this._mapper.Map<SourceGetRp>(entity);
            if (entity!= null) {
                var sourceItems = await this._dbContext.GetSourceItems(entity.Id.Value, start, end);

                var ids = sourceItems.Select(c => c.Id).ToList();
                
                entity.SourceItems = sourceItems;                                
                result.Quality = entity.Measure();                
                result.Features = entity.FeaturesToDictionary();
            }            
            return result;
        }        

        public async Task<IEnumerable<SourceGetListRp>> GetByProductId(int productId)
        {
            var entities = await this._dbContext.Sources.Where(c => c.Product.Id == productId).ToListAsync();
            return this._mapper.Map<IEnumerable<SourceGetListRp>>(entities);
        }
   
        public async Task<SourcesGetRp> GetByProductIdWithAvailability(int productId, DateTime start, DateTime end)
        {
            var entities = await this._dbContext.Sources.Include(c=>c.Indicators).Where(c => c.Product.Id == productId).ToListAsync();            
            var sourceItems = await this._dbContext.GetSourceItems(start, end);

            var items = new List<SourceGetListRp>();
            foreach (var source in entities)
            {                
                source.SourceItems = sourceItems.Where(c => c.SourceId == source.Id).ToList();                
                var proportion= source.Measure();
                var tmp = this._mapper.Map<SourceGetListRp>(source);
                tmp.References = source.Indicators.Count();
                tmp.Correlation = source.MeasureDailyCorrelation();
                tmp.Measure = proportion;                
                items.Add(tmp);
            }
            var model = new SourcesGetRp();
            model.Items = items.ToList();
            var agg = new  SourceMetricsAggregate(entities);            
                
            var quality = agg.Execute();
            model.Availability = quality.Availability;
            model.Experience = quality.Experience;
            model.Latency = quality.Latency;
            model.AvailabilityInteractionsTotal = quality.Total;
            model.AvailabilityInteractionsGood = quality.Good;

            return model;            
        }

        public async Task<IEnumerable<SourceGetListRp>> GetByIndicatorId(int indicatorId)
        {
            var entities = await this._dbContext.Indicators.Include(c=>c.Source).Where(c=>c.Id == indicatorId).ToListAsync();
            var targets = entities.Select(c => c.Source).ToList();
            return this._mapper.Map<IEnumerable<SourceGetListRp>>(targets);
        }
        public async Task<SourceGetRp> GetByIdWithDetail(int id, DatePeriodValue period)
        {
            var entity = await this._dbContext.Sources
              .Include(c => c.Indicators)
              .ThenInclude(c => c.Feature)
              .SingleOrDefaultAsync(c => c.Id == id);

            var result = this._mapper.Map<SourceGetRp>(entity);
            if (entity != null)
            {
                var sourceItems = await this._dbContext.GetSourceItems(entity.Id.Value, period.Start, period.End);

                var ids = sourceItems.Select(c => c.Id).ToList();

                entity.SourceItems = sourceItems;                
                result.Quality = entity.Measure();
                result.Features = entity.FeaturesToDictionary();
            }
            return result;
        }

        public async Task<SeriesGetRp> GetDailySeriesById(int sourceId,
            DatePeriodValue period) {                                   
            var source = await this._dbContext.Sources.SingleAsync(c => c.Id == sourceId);
            var sourceItems = await this._dbContext.GetSourceItems(sourceId, period.Start, period.End);
            source.SourceItems = sourceItems;

            var result = new SeriesGetRp
            {
                Start = period.Start,
                End = period.End,
                Name = source.Name,
                Avatar = source.Avatar
            };
            var aggregator = new SourceDailyAvailabilityAggregate(source, period);
            var items = aggregator.MeasureAvailability();
            result.Items = items.Select(c=> new SeriesItemGetRp(c.Date, c.Measure.Availability)).ToList();
            return result;
        }
    }
}
