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
    public class SquadComponent : BaseComponent, ISquadComponent
    {
        private readonly ISquadRepository _squadRepository;
        private readonly IUserIdentityService _identityService;

        public SquadComponent(ISquadRepository squadRepository,
            IUserIdentityService identityService)
        {
            this._squadRepository = squadRepository;
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
