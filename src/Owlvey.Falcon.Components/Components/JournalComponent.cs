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
    public class JournalComponent : BaseComponent, IJournalComponent
    {
        private readonly FalconDbContext _dbContext;        

        public JournalComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityService, IDateTimeGateway dateTimeGateway, IMapper mapper) : base(dateTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;            
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
