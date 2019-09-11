using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Core;
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
    public class SquadQueryComponent : BaseComponent
    {
        private readonly FeatureQueryComponent _featureQueryComponent;

        private readonly FalconDbContext _dbContext;
        public SquadQueryComponent(FalconDbContext dbContext, IDateTimeGateway dateTimeGateway, IMapper mapper,
            IUserIdentityGateway identityService,
            FeatureQueryComponent featureQueryComponent) : base(dateTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
            this._featureQueryComponent = featureQueryComponent;
        }

        /// <summary>
        /// Get Squad by id
        /// </summary>
        /// <param name="key">Squad Id</param>
        /// <returns></returns>
        public async Task<SquadGetRp> GetSquadById(int id)
        {
            var entity = await this._dbContext.Squads
                .Include(c=>c.Members).ThenInclude(c=>c.User)
                .Include(c => c.Features).ThenInclude(c => c.Feature)
                                         .ThenInclude(c => c.Product)
                .SingleOrDefaultAsync(c=>c.Id == id);

            if (entity == null)
                return null;

            return this._mapper.Map<SquadGetRp>(entity);
        }

        public async Task<SquadGetDetailRp> GetSquadByIdWithAvailability(int id, DateTime start, DateTime end)
        {
            var entity = await this._dbContext.Squads
                .Include(c => c.Members).ThenInclude(c => c.User)
                .Include(c => c.Features).ThenInclude(c => c.Feature)
                        .ThenInclude(c=>c.ServiceMaps).ThenInclude(c=>c.Service).ThenInclude(c=>c.Product)
                .SingleOrDefaultAsync(c => c.Id == id);

            var common = new FeatureCommonComponent(this._dbContext, this._datetimeGateway);                   

            SquadGetDetailRp result = this._mapper.Map<SquadGetDetailRp>(entity);            

            foreach (var featureMap in entity.Features)
            {
                //var service = featureMap.Feature.ServiceMaps.
                var feature = featureMap.Feature;
                var (_, availabilty) = await common.GetFeatureByIdWithAvailability(featureMap.FeatureId, start, end);

                foreach (var serviceMap in featureMap.Feature.ServiceMaps)
                {
                    var service = serviceMap.Service;
                    var tmp = new FeatureBySquadRp()
                    {
                        Id = feature.Id.Value,
                        Description = feature.Description,
                        Avatar = feature.Avatar,
                        CreatedBy = feature.CreatedBy,
                        CreatedOn = feature.CreatedOn,
                        Availability = availabilty,
                        ProductId = service.ProductId,
                        Product = service.Product.Name,
                        ServiceId = service.Id.Value,
                        ServiceAvatar = service.Avatar,
                        Service = service.Name,
                        SLO = service.Slo,
                        Name = feature.Name,
                        MTBF = feature.MTBF,
                        MTTD = feature.MTTD,
                        MTTF = feature.MTTF,
                        MTTR = feature.MTTR,
                        Impact = AvailabilityUtils.MeasureImpact(service.Slo),
                        Points = AvailabilityUtils.MeasurePoints(service.Slo, availabilty)
                    };
                    result.Points += tmp.Points;
                    result.Features.Add(tmp);
                }
            }

            result.Features = result.Features.OrderBy(c => c.Service).ToList();
            return result;
        }

        public async Task<SquadGetRp> GetSquadByName(int customerId, string name)
        {
            var entity = await this._dbContext.Squads.SingleOrDefaultAsync(c => c.Customer.Id == customerId && c.Name == name );

            return this._mapper.Map<SquadGetRp>(entity);
        }

        /// <summary>
        /// Get All Squad
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SquadGetListRp>> GetSquads(int customerId)
        {
            var entities = await this._dbContext.Squads
                .Where(c=> c.Customer.Id.Equals(customerId)).ToListAsync();

            return this._mapper.Map<IEnumerable<SquadGetListRp>>(entities);
        }

        
        public async Task<IEnumerable<SquadGetListRp>> GetSquadsWithPoints(int customerId, 
            DateTime start, 
            DateTime end)
        {
            var entities = await this._dbContext.Squads
                .Where(c => c.Customer.Id.Equals(customerId)).ToListAsync();
            List<SquadGetListRp> result = new List<SquadGetListRp>();
            foreach (var squad in entities)
            {
                var tmp = await this.GetSquadByIdWithAvailability(squad.Id.Value, start, end);
                
                result.Add(new SquadGetListRp() {
                     Id = tmp.Id,
                     Name = tmp.Name,
                     Avatar = tmp.Avatar,
                     Points = tmp.Points,
                     Features = tmp.Features.Count()
                });
            }
            return result;
        }


    }
}
