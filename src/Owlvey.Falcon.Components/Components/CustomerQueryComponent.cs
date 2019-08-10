using Owlvey.Falcon.Components;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Core.Aggregates;

namespace Owlvey.Falcon.Components
{
    public class CustomerQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;        
        public CustomerQueryComponent(FalconDbContext dbContext, IMapper mapper, IDateTimeGateway dateTimeGateway, IUserIdentityGateway identityService) : base(dateTimeGateway, mapper, identityService)
        {            
            this._dbContext = dbContext;
        }


        public async Task<CustomerGetRp> GetCustomerById(int id)
        {
            var entities = await this._dbContext.Customers.Where(c => c.Id.Equals(id)).ToListAsync();

            var entity = entities.FirstOrDefault();            

            if (entity == null)
                return null;

            return this._mapper.Map<CustomerGetRp>(entity);
        }

        public async Task<CustomerGetRp> GetCustomerByName(string name)
        {
            var entity = await this._dbContext.Customers.SingleAsync(c => c.Name.Equals(name));
            return this._mapper.Map<CustomerGetRp>(entity);
        }



        /// <summary>
        /// Get All Customer
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CustomerGetListRp>> GetCustomers()
        {            
            var entities = await this._dbContext.Customers.ToListAsync();

            return entities.Select(entity => new CustomerGetListRp {
                Id = entity.Id.Value,
                Name = entity.Name,
                Avatar = entity.Avatar,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            });
        }

        public async Task<SeriesGetRp> GetDailySeriesById(int productId, DateTime start, DateTime end)
        {
            var entity = await this._dbContext.Customers.Include(c => c.Products.Select(g=>g.Services.Select(d => d.FeatureMap.Select(f => f.Feature.Indicators.Select(e => e.Source))))).SingleAsync(c => c.Id == productId);

            foreach (var product in entity.Products)
            {
                foreach (var service in product.Services)
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
            }            
                                   
            var result = new SeriesGetRp
            {
                Start = start,
                End = end,
                Name = entity.Name,
                Avatar = entity.Avatar
            };

            var aggregator = new CustomerAvailabilityAggregate(entity, start, end);
            var (_, items) = aggregator.MeasureAvailability();

            foreach (var item in items)
            {
                result.Items.Add(this._mapper.Map<SeriesItemGetRp>(item));
            }

            return result;
        }
    }
}
