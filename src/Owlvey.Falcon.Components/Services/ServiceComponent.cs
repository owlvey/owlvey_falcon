using Owlvey.Falcon.Components.Gateways;
using Owlvey.Falcon.Components.Interfaces;
using Owlvey.Falcon.Components.Models;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Components.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components.Services
{
    public class ServiceComponent : BaseComponent, IServiceComponent
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IUserIdentityService _identityService;

        public ServiceComponent(IServiceRepository serviceRepository,
            IUserIdentityService identityService)
        {
            this._serviceRepository = serviceRepository;
            this._identityService = identityService;
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
