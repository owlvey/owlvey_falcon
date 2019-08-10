using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public class ServiceQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;

        public ServiceQueryComponent(FalconDbContext dbContext, IDateTimeGateway dateTimeGateway, IMapper mapper, IUserIdentityGateway identityService) : base(dateTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
        }

        public async Task<ServiceGetRp> GetServiceById(int id)
        {
            var entity = await this._dbContext.Services.FirstOrDefaultAsync(c=> c.Id.Equals(id));

            if (entity == null)
                return null;

            return this._mapper.Map<ServiceGetRp>(entity);
        }
        public async Task<ServiceGetRp> GetServiceByName(int productId, string name)
        {
            var entity = await this._dbContext.Services.FirstOrDefaultAsync(c => c.Product.Id == productId && c.Name == name);
            return this._mapper.Map<ServiceGetRp>(entity);
        }

        public async Task<IEnumerable<ServiceGetListRp>> GetServices(int productId)
        {
            var entities = await this._dbContext.Services.Where(c=> c.Product.Id.Equals(productId)).ToListAsync();
            return this._mapper.Map<IEnumerable<ServiceGetListRp>>(entities);
        }

        public async Task<SeriesGetRp> GetDailySeriesById(int featureId, DateTime start, DateTime end)
        {            
            var entity = await this._dbContext.Services.Include(c => c.FeatureMap.Select(d=>d.Feature.Indicators.Select(e=>e.Source.SourceItems))).SingleAsync(c => c.Id == featureId);

            var result = new SeriesGetRp
            {
                Start = start,
                End = end,
                Name = entity.Name
            };

            var aggregator = new ServiceAvailabilityAggregate(entity, start, end);

            var (_, items) = aggregator.MeasureAvailability();

            foreach (var item in items)
            {
                result.Items.Add(this._mapper.Map<SeriesItemGetRp>(item));
            }

            return result;
        }
    }
}
