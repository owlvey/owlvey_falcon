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
    public class FeatureComponent : BaseComponent, IFeatureComponent
    {
        private readonly FalconDbContext _dbContext;
        private readonly IUserIdentityGateway _identityService;

        public FeatureComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityService, IDateTimeGateway dateTimeGateway, IMapper mapper) : base(dateTimeGateway, mapper)
        {
            this._dbContext = dbContext;
            this._identityService = identityService;
        }

        /// <summary>
        /// Create a new Feature
        /// </summary>
        /// <param name="model">Feature Model</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> CreateFeature(FeaturePostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();




            return result;
        }

        /// <summary>
        /// Delete Feature
        /// </summary>
        /// <param name="key">Feature Id</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> DeleteFeature(int id)
        {
            var result = new BaseComponentResultRp();

            return result;
        }
        
        /// <summary>
        /// Update Feature
        /// </summary>
        /// <param name="model">Feature Model</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> UpdateFeature(int id, FeaturePutRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();

            return result;
        }
    }
}
