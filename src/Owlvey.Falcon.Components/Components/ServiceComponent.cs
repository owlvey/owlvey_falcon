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

        /// <summary>
        /// Create a new Service
        /// </summary>
        /// <param name="model">Service Model</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> CreateService(ServicePostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();

            var product = await this._dbContext.Products.SingleAsync(c => c.Id == model.ProductId);
                       
            var service = ServiceEntity.Factory.Create(model.Name, model.SLO, this._datetimeGateway.GetCurrentDateTime(), createdBy, product);

            this._dbContext.Services.Add(service);

            await this._dbContext.SaveChangesAsync();
            
            return result;
        }

        /// <summary>
        /// Delete Service
        /// </summary>
        /// <param name="key">Service Id</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> DeleteService(int id)
        {
            var result = new BaseComponentResultRp();

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

            return result;
        }
    }
}
