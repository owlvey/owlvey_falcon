using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Owlvey.Falcon.Components
{
    public class SquadComponent : BaseComponent, ISquadComponent
    {
        private readonly FalconDbContext _dbContext;        

        public SquadComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityService, IDateTimeGateway dateTimeGateway, IMapper mapper) : base(dateTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;            
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
            var customer = await this._dbContext.Customers.SingleAsync(c => c.Id == model.CustomerId);
            var entity = SquadEntity.Factory.Create(model.Name, this._datetimeGateway.GetCurrentDateTime(), createdBy, customer);
            this._dbContext.Squads.Add(entity);
            await this._dbContext.SaveChangesAsync();
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
