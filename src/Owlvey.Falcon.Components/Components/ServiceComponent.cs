using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Owlvey.Falcon.Components
{
    public class ServiceComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;

        public ServiceComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityService, IDateTimeGateway dateTimeGateway, IMapper mapper) : base(dateTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
        }

        public async Task<ServiceGetListRp> CreateOrUpdate(CustomerEntity customer,
            string product, string name, string description, string avatar, decimal slo)
        {
            var createdBy = this._identityService.GetIdentity();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            var entity = await this._dbContext.Services.Where(c => c.Product.CustomerId == customer.Id
                                     && c.Product.Name == product && c.Name == name).SingleOrDefaultAsync();
            if (entity == null)
            {
                var productEntity = await this._dbContext.Products.Where(c => c.CustomerId == customer.Id && c.Name == product).SingleAsync();
                entity = ServiceEntity.Factory.Create(name, this._datetimeGateway.GetCurrentDateTime(), createdBy, productEntity);
            }

            entity.Update(this._datetimeGateway.GetCurrentDateTime(), createdBy, name, slo, description, avatar);

            this._dbContext.Services.Update(entity);
            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<ServiceGetListRp>(entity);
        }

        /// <summary>
        /// Create a new Service
        /// </summary>
        /// <param name="model">Service Model</param>
        /// <returns></returns>
        public async Task<ServiceGetListRp> CreateService(ServicePostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();

            var entity = await this._dbContext.Services.Where(c => c.ProductId == model.ProductId && c.Name == model.Name).SingleOrDefaultAsync();
            if (entity == null)
            {
                var product = await this._dbContext.Products.Where(c => c.Id == model.ProductId).SingleAsync();
                entity = ServiceEntity.Factory.Create(model.Name, this._datetimeGateway.GetCurrentDateTime(),
                    createdBy, product);
                this._dbContext.Services.Add(entity);
                await this._dbContext.SaveChangesAsync();
            }
            return this._mapper.Map<ServiceGetListRp>(entity);
        }

        /// <summary>
        /// Delete Service
        /// </summary>
        /// <param name="key">Service Id</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> DeleteService(int id)
        {
            var result = new BaseComponentResultRp();
            var modifiedBy = this._identityService.GetIdentity();

            var service = await this._dbContext.Services.SingleAsync(c => c.Id == id);

            if (service == null)
            {
                result.AddNotFound($"The Resource {id} doesn't exists.");
                return result;
            }

            service.Delete(this._datetimeGateway.GetCurrentDateTime(), modifiedBy);

            this._dbContext.Services.Remove(service);

            await this._dbContext.SaveChangesAsync();

            return result;
        }

        /// <summary>
        /// Update Service
        /// </summary>
        /// <param name="model">Service Model</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> UpdateService(int id, ServicePutRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();

            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            var service = await this._dbContext.Services.Include(c => c.Product).SingleAsync(c => c.Id == id);

            if (service == null)
            {
                result.AddNotFound($"The Resource {id} doesn't exists.");
                return result;
            }

            // Validate if the resource exists.
            if (!service.Name.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                var product = await this._dbContext.Products.Include(c => c.Services).SingleAsync(c => c.Id.Equals(service.Product.Id));

                if (product.Services.Any(c => c.Name.Equals(model.Name)))
                {
                    result.AddConflict($"The Resource {model.Name} has already been taken.");
                    return result;
                }
            }
                        
            service.Update(this._datetimeGateway.GetCurrentDateTime(), createdBy, model.Name, model.Slo, model.Avatar);

            this._dbContext.Services.Update(service);

            await this._dbContext.SaveChangesAsync();

            return result;
        }
    }
}
