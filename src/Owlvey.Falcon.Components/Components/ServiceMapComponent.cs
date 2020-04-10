﻿using System;
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
using Polly;
//
namespace Owlvey.Falcon.Components
{
    public class ServiceMapComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;

        public ServiceMapComponent(FalconDbContext dbContext, IDateTimeGateway dataTimeGateway, IMapper mapper, IUserIdentityGateway identityService,
            ConfigurationComponent configuration) : base(dataTimeGateway, mapper, identityService, configuration)
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
            var featureEntity = await this._dbContext.Features.Where(c => c.ProductId == productEntity.Id && c.Name == feature).SingleAsync();
            await this.CreateServiceMap(new ServiceMapPostRp()
            {
                FeatureId = featureEntity.Id,
                ServiceId = serviceEntity.Id
            });           
        }

        public async Task CreateServiceMap(ServiceMapPostRp model)
        {            
            var createdBy = this._identityService.GetIdentity();

            var retryPolicy = Policy.Handle<DbUpdateException>()
                .WaitAndRetryAsync(this._configuration.DefaultRetryAttempts,
                i => this._configuration.DefaultPauseBetweenFails);

            await retryPolicy.ExecuteAsync(async () =>
            {
                var entity = await this._dbContext.ServiceMaps.Where(c => c.ServiceId == model.ServiceId && c.FeatureId == model.FeatureId).SingleOrDefaultAsync();

                if (entity == null)
                {
                    var service = await this._dbContext.Services.SingleAsync(c => c.Id == model.ServiceId);
                    var feature = await this._dbContext.Features.SingleAsync(c => c.Id == model.FeatureId);
                    entity = ServiceMapEntity.Factory.Create(service, feature, this._datetimeGateway.GetCurrentDateTime(), createdBy);
                    await this._dbContext.AddAsync(entity);
                    await this._dbContext.SaveChangesAsync();
                }                
            });
        }
        public async Task<IEnumerable<ServiceGetListRp>> GetServiceMaps(int serviceId)
        {
            var entities = await this._dbContext.ServiceMaps.Where(c => c.Service.Id == serviceId).Select(c => c.Service).ToListAsync();  
            return this._mapper.Map<IEnumerable<ServiceGetListRp>>(entities);
        }
    }
}
