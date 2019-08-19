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
            var entities = await this._dbContext.Services.Include(c=>c.FeatureMap).Where(c=> c.Product.Id.Equals(productId)).ToListAsync();
            return this._mapper.Map<IEnumerable<ServiceGetListRp>>(entities);
        }

        public async Task<MultiSeriesGetRp> GetDailySeriesById(int serviceId, DateTime start, DateTime end)
        {
            var service = await this._dbContext.Services.Where(c => c.Id == serviceId).SingleAsync();
            var serviceMaps = await this._dbContext.ServiceMaps.Include(c => c.Feature).ThenInclude(c=>c.Indicators).Where(c => c.Service.Id == serviceId).ToListAsync();

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
            
            var result = new MultiSeriesGetRp
            {
                Start = start,
                End = end,
                Name = service.Name,
                Avatar = service.Avatar
            };

            var aggregator = new ServiceAvailabilityAggregate(service, start, end);
            
            var (_, availability, features) = aggregator.MeasureAvailability();

            result.Series.Add(new MultiSerieItemGetRp()
            {
                Name = "Availability",
                Avatar = service.Avatar,
                Items = availability.Select(c => this._mapper.Map<SeriesItemGetRp>(c)).ToList()
            });

            foreach (var indicator in features)
            {
                result.Series.Add(new MultiSerieItemGetRp()
                {
                    Name = string.Format("SLI:{0}", indicator.Item1.Id),
                    Avatar = indicator.Item1.Avatar,
                    Items = indicator.Item2.Select(c => this._mapper.Map<SeriesItemGetRp>(c)).ToList()
                });
            }

            return result;
        }
    }
}
