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

namespace Owlvey.Falcon.Components
{
    public class SquadComponent : BaseComponent, ISquadComponent
    {
        private readonly FalconDbContext _dbContext;
        private readonly IUserIdentityGateway _identityService;

        public SquadComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityService)
        {
            this._dbContext = dbContext;
            this._identityService = identityService;
        }

        /// <summary>
        /// Create a new Squad
        /// </summary>
        /// <param name="model">Squad Model</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> CreateSquad(SquadPostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();


            return result;
        }

        /// <summary>
        /// Delete Squad
        /// </summary>
        /// <param name="key">Squad Id</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> DeleteSquad(int id)
        {
            var result = new BaseComponentResultRp();

            return result;
        }
        
        /// <summary>
        /// Update Squad
        /// </summary>
        /// <param name="model">Squad Model</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> UpdateSquad(int id, SquadPutRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();

            return result;
        }
    }
}
