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
    public class JournalComponent : BaseComponent, IJournalComponent
    {
        private readonly IJournalRepository _journalRepository;
        private readonly IUserIdentityService _identityService;

        public JournalComponent(IJournalRepository journalRepository,
            IUserIdentityService identityService)
        {
            this._journalRepository = journalRepository;
            this._identityService = identityService;
        }

        /// <summary>
        /// Create a new Journal
        /// </summary>
        /// <param name="model">Journal Model</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> CreateJournal(JournalPostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();


            return result;
        }

        /// <summary>
        /// Delete Journal
        /// </summary>
        /// <param name="key">Journal Id</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> DeleteJournal(int id)
        {
            var result = new BaseComponentResultRp();

            return result;
        }
        
        /// <summary>
        /// Update Journal
        /// </summary>
        /// <param name="model">Journal Model</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> UpdateJournal(int id, JournalPutRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();

            return result;
        }
    }
}
