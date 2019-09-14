using System;
using System.Threading.Tasks;
using AutoMapper;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Core.Entities;
using System.Collections.Generic;
using System.Diagnostics;
//
namespace Owlvey.Falcon.Components
{
    public class ServiceMapComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;

        public ServiceMapComponent(FalconDbContext dbContext, IDateTimeGateway dataTimeGateway, IMapper mapper, IUserIdentityGateway identityService) : base(dataTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
        }

        public async Task<BaseComponentResultRp> DeleteServiceMap(int serviceId, int featureId) {
            var result = new BaseComponentResultRp();
            var entity = await this._dbContext.ServiceMaps.Where(c => c.ServiceId == serviceId && c.FeatureId == featureId ).SingleOrDefaultAsync();
            if (entity != null) {
                this._dbContext.ServiceMaps.Remove(entity);
                await this._dbContext.SaveChangesAsync();
            }            
            return result;
        }

        public async Task CreateServiceMap(int customerId, string product, string service, string feature ) {
            var productEntity = await this._dbContext.Products.Where(c => c.CustomerId == customerId && c.Name == product).SingleAsync();
            var serviceEntity = await this._dbContext.Services.Where(c => c.ProductId == productEntity.Id && c.Name == service).SingleAsync();
            try
            {
                var featureEntity = await this._dbContext.Features.Where(c => c.ProductId == productEntity.Id && c.Name == feature).SingleAsync();

                await this.CreateServiceMap(new ServiceMapPostRp()
                {
                    FeatureId = featureEntity.Id,
                    ServiceId = serviceEntity.Id
                });
            }
            catch (Exception)
            {
                Debugger.Break(); 
                throw;
            }
            
        }


        public async Task<BaseComponentResultRp> CreateServiceMap(ServiceMapPostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();
            
            var previous = await this._dbContext.ServiceMaps.Where(c => c.ServiceId == model.ServiceId && c.FeatureId == model.FeatureId).SingleOrDefaultAsync();

            if (previous != null) {
                return result;
            }

            var service = await this._dbContext.Services.SingleAsync(c => c.Id == model.ServiceId);
            var feature = await this._dbContext.Features.SingleAsync(c => c.Id == model.FeatureId);

            var entity = ServiceMapEntity.Factory.Create(service, feature, this._datetimeGateway.GetCurrentDateTime(), createdBy);

            await this._dbContext.AddAsync(entity);
            await this._dbContext.SaveChangesAsync();

            result.AddResult("Id", entity.Id);

            return result;
        }
        public async Task<IEnumerable<ServiceGetListRp>> GetServiceMaps(int serviceId)
        {
            var entities = await this._dbContext.ServiceMaps.Where(c => c.Service.Id == serviceId).Select(c => c.Service).ToListAsync();  
            return this._mapper.Map<IEnumerable<ServiceGetListRp>>(entities);
        }
    }
}
