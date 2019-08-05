using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Owlvey.Falcon.Components
{
    public class ServiceComponent : BaseComponent, IServiceComponent
    {
        private readonly FalconDbContext _dbContext;
        private readonly IUserIdentityGateway _identityService;

        public ServiceComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityService, IDateTimeGateway dateTimeGateway, IMapper mapper) : base(dateTimeGateway, mapper)
        {
            this._dbContext = dbContext;
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
