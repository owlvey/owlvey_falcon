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
            var entity = await this._dbContext.Products.SingleOrDefaultAsync(c=> c.Id.Equals(id));
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


        public async Task<MultiSeriesGetRp> GetDailySeriesById(int productId, DateTime start, DateTime end)
        {
            var product = await this._dbContext.Products.Include(c=>c.Services).Where(c => c.Id == productId).SingleAsync();

            foreach (var service in product.Services)
            {
                var serviceMaps = await this._dbContext.ServiceMaps.Include(c => c.Feature).ThenInclude(c => c.Indicators).Where(c => c.Service.Id == service.Id).ToListAsync();
                foreach (var map in serviceMaps)
                {
                    var entity = await this._dbContext.Features.Include(c => c.Indicators).ThenInclude(c => c.Source).SingleAsync(c => c.Id == map.Feature.Id);

                    foreach (var indicator in entity.Indicators)
                    {                        
                        var sourceItems = await this._dbContext.GetSourceItems(indicator.SourceId, start, end);
                        indicator.Source.SourceItems = sourceItems;
                    }
                    map.Feature = entity;
                }
                service.FeatureMap = serviceMaps;

            }

            var result = new MultiSeriesGetRp
            {
                Start = start,
                End = end,
                Name = product.Name,
                Avatar = product.Avatar
            };

            var aggregator = new ProductAvailabilityAggregate(product, start, end);

            var (_, availability, features) = aggregator.MeasureAvailability();

            result.Series.Add(new MultiSerieItemGetRp()
            {
                Name = "Availability",
                Avatar = product.Avatar,
                Items = availability.Select(c => this._mapper.Map<SeriesItemGetRp>(c)).ToList()
            });

            foreach (var indicator in features)
            {
                result.Series.Add(new MultiSerieItemGetRp()
                {
                    Name = string.Format("Service:{0}", indicator.Item1.Id),
                    Avatar = indicator.Item1.Avatar,
                    Items = indicator.Item2.Select(c => this._mapper.Map<SeriesItemGetRp>(c)).ToList()
                });
            }

            return result;

        }
    }
}
