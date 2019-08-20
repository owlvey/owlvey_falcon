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
        public async Task<BaseComponentResultRp> Create(SourcePostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();

            var product = await this._dbContext.Products.SingleAsync(c => c.Id == model.ProductId);
            var entity = SourceEntity.Factory.Create(product, model.Name, this._datetimeGateway.GetCurrentDateTime(), createdBy);
            
            await this._dbContext.AddAsync(entity);

            await this._dbContext.SaveChangesAsync();

            result.AddResult("Id", entity.Id);

            return result;
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
        public async Task<SourceGetRp> GetByIdWithAvailability(int id, DateTime end)
        {
            var entity = await this._dbContext.Sources.SingleOrDefaultAsync(c => c.Id == id);
            var result = this._mapper.Map<SourceGetRp>(entity);
            if (entity!= null) {
                result.Availability = await GetAvailabilityBySource(entity, end);
            }
            
            return result;
        }

        private async Task<decimal> GetAvailabilityBySource(SourceEntity entity, DateTime end) {
            var sourceItems = await this._dbContext.GetSourceItems(entity.Id.Value, end, end);
            entity.SourceItems = sourceItems;
            var agg = new SourceDateAvailabilityAggregate(entity);            
            return agg.MeasureAvailability();
        }

        public async Task<IEnumerable<SourceGetListRp>> GetByProductId(int productId)
        {
            var entities = await this._dbContext.Sources.Where(c => c.Product.Id == productId).ToListAsync();
            return this._mapper.Map<IEnumerable<SourceGetListRp>>(entities);
        }
        public async Task<IEnumerable<SourceGetListRp>> GetByProductIdWithAvailability(int productId, DateTime end)
        {
            var entities = await this._dbContext.Sources.Where(c => c.Product.Id == productId).ToListAsync();
            var result = new List<SourceGetListRp>();
            foreach (var item in entities)
            {
                var tmp = this._mapper.Map<SourceGetListRp>(item);
                tmp.Availability = await this.GetAvailabilityBySource(item, end);
                result.Add(tmp);
            }
            return result;
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

            var aggregator = new SourceAvailabilityAggregate(source, start, end);
            var (_, items) = aggregator.MeasureAvailability();                        

            foreach (var item in items)
            {
                result.Items.Add(this._mapper.Map<SeriesItemGetRp>(item));                
            }            
            
            return result;
        }
    }
}
