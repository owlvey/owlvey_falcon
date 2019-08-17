using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public class ProductQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;        

        public ProductQueryComponent(FalconDbContext dbContext, IDateTimeGateway dateTimeGateway,
            IMapper mapper, IUserIdentityGateway identityService) : base(dateTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
        }

        public async Task<ProductGetRp> GetProductByName(string Name)
        {
            var entity = await this._dbContext.Products.SingleAsync(c => c.Name == Name);
            return this._mapper.Map<ProductGetRp>(entity);
        }

        public async Task<ProductGetRp> GetProductById(int id)
        {
            var entity = await this._dbContext.Products.SingleAsync(c=> c.Id.Equals(id));
            return this._mapper.Map<ProductGetRp>(entity);
        }

        /// <summary>
        /// Get All Product
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ProductGetListRp>> GetProducts(int customerId)
        {
            var entities = await this._dbContext.Products.Where(c => c.Customer.Id == customerId).ToListAsync();
            return this._mapper.Map<IEnumerable<ProductGetListRp>>(entities);            
        }
        public async Task<IEnumerable<ProductGetListRp>> GetProductsWithServices(int customerId) {
            var entities = await this._dbContext.Products.Include(c => c.Services).Where(c => c.Customer.Id == customerId).ToListAsync();
            return this._mapper.Map<IEnumerable<ProductGetListRp>>(entities);
        }


        public async Task<SeriesGetRp> GetDailySeriesById(int featureId, DateTime start, DateTime end)
        {
            var entity = await this._dbContext.Products.Include(c => c.Services.Select(d => d.FeatureMap.Select(f=>f.Feature.Indicators.Select(e => e.Source)))).SingleAsync(c => c.Id == featureId);

            foreach (var service in entity.Services)
            {
                foreach (var feature in service.FeatureMap)
                {
                    foreach (var indicator in feature.Feature.Indicators)
                    {
                        var sourceItems = await this._dbContext.SourcesItems.Where(c => c.SourceId == indicator.Source.Id && c.Start >= start && c.End <= end).ToListAsync();
                        indicator.Source.SourceItems = sourceItems;
                    }
                }                
            }

            var result = new SeriesGetRp
            {
                Start = start,
                End = end,
                Name = entity.Name,
                Avatar = entity.Avatar
            };

            var aggregator = new ProductAvailabilityAggregate(entity, start, end);

            var (_, items) = aggregator.MeasureAvailability();

            foreach (var item in items)
            {
                result.Items.Add(this._mapper.Map<SeriesItemGetRp>(item));
            }

            return result;
        }
    }
}
