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
    public class SquadComponent : BaseComponent
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

            // Validate if the resource exists.
            if (customer.Squads.Any(c => c.Name.Equals(model.Name)))
            {
                result.AddConflict($"The Resource {model.Name} has already been taken.");
                return result;
            }

            var entity = SquadEntity.Factory.Create(model.Name, this._datetimeGateway.GetCurrentDateTime(), createdBy, customer);
            this._dbContext.Squads.Add(entity);
            await this._dbContext.SaveChangesAsync();

            result.AddResult("Id", entity.Id);

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
            var modifiedBy = this._identityService.GetIdentity();

            var squad = await this._dbContext.Squads.SingleAsync(c => c.Id == id);

            if (squad == null)
            {
                result.AddNotFound($"The Resource {id} doesn't exists.");
                return result;
            }

            squad.Delete(this._datetimeGateway.GetCurrentDateTime(), modifiedBy);

            this._dbContext.Squads.Remove(squad);

            await this._dbContext.SaveChangesAsync();

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

            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            var squad = await this._dbContext.Squads.Include(c=> c.Customer).SingleAsync(c => c.Id == id);

            if (squad == null)
            {
                result.AddNotFound($"The Resource {id} doesn't exists.");
                return result;
            }

            // Validate if the resource exists.
            if (!squad.Name.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase))
            {

                var customer = await this._dbContext.Customers.Include(c=> c.Squads).SingleAsync(c => c.Id.Equals(squad.Customer.Id));

                if (customer.Squads.Any(c => c.Name.Equals(model.Name)))
                {
                    result.AddConflict($"The Resource {model.Name} has already been taken.");
                    return result;
                }
            }

            squad.Update(this._datetimeGateway.GetCurrentDateTime(), createdBy, model.Name, model.Description, model.Avatar);

            this._dbContext.Update(squad);

            await this._dbContext.SaveChangesAsync();

            return result;
        }
    }
}
