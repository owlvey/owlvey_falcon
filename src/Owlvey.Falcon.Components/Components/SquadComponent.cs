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
        public async Task<SquadGetRp> CreateSquad(SquadPostRp model)
        {            
            var createdBy = this._identityService.GetIdentity();
            var customer = await this._dbContext.Customers.Where(c => c.Id == model.CustomerId).SingleAsync();
                        
            var entity = await this._dbContext.Squads.Where(c => c.Name == model.Name && c.CustomerId == model.CustomerId).SingleOrDefaultAsync();
            if (entity == null) {
                entity = SquadEntity.Factory.Create(model.Name, this._datetimeGateway.GetCurrentDateTime(), createdBy, customer);
                this._dbContext.Squads.Add(entity);
                await this._dbContext.SaveChangesAsync();
            }                                    

            return this._mapper.Map<SquadGetRp>(entity);
        }                       

        public async Task<SquadGetRp> CreateOrUpdate(CustomerEntity customer, string name, string description, string avatar
            , string leaders) {
            var createdBy = this._identityService.GetIdentity();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            var entity = await this._dbContext.Squads.Where(c => c.CustomerId == customer.Id && c.Name == name).SingleOrDefaultAsync();
            if (entity == null) {
                entity = SquadEntity.Factory.Create(name, this._datetimeGateway.GetCurrentDateTime(), createdBy, customer);
            }
            entity.Update(this._datetimeGateway.GetCurrentDateTime(), createdBy, name, description, avatar, leaders);
            this._dbContext.Squads.Update(entity);
            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<SquadGetRp>(entity);           
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

            this._dbContext.Squads.Remove(squad);

            await this._dbContext.SaveChangesAsync();

            return result;
        }

        public async Task RegisterMember(int customerId, string name, int userId) {
            var squad = await this._dbContext.Squads.Where(c => c.CustomerId == customerId && c.Name == name).SingleAsync();
            await this.RegisterMember(squad.Id.Value, userId);
        }

        public async Task RegisterMember(int id, int userId) {
            var exists = await this._dbContext.Members.Where(c => c.UserId == userId && c.SquadId == id).SingleOrDefaultAsync();
            if (exists == null) {
                var createdBy = this._identityService.GetIdentity();
                var member = MemberEntity.Factory.Create(id, userId, this._datetimeGateway.GetCurrentDateTime(), createdBy);
                this._dbContext.Members.Add(member);
                await this._dbContext.SaveChangesAsync();
            }
        }
        
        public async Task<IEnumerable<UserGetListRp>> GetUsersComplement(int squadId)
        {
            var result = new List<SquadGetListRp>();
            var squad = await this._dbContext.Squads.Include(c => c.Members)
                .ThenInclude(c => c.User).Where(c => c.Id == squadId).SingleAsync();

            var users = await this._dbContext.Users.ToListAsync();
            
            var squadUsers = squad.Members.Select(c => c.User).ToList();

            var diff = users.Except(squadUsers, new UserEntityCompare());

            return this._mapper.Map<IEnumerable<UserGetListRp>>(diff);
        }

        public async Task<IEnumerable<SquadGetListRp>> GetSquadComplementByFeature(int featureId)
        {

            var result = new List<SquadGetListRp>();
            var feature = await this._dbContext.Features.Include(c=>c.Product).Include(c=>c.Squads).ThenInclude(c=>c.Squad).Where(c => c.Id == featureId).SingleAsync();

            var squads = await this._dbContext.Squads.Where(c => c.CustomerId == feature.Product.CustomerId).ToListAsync();

            var featureSquads = feature.Squads.Select(c => c.Squad).ToList();

            var diff = squads.Except(featureSquads, new SquadCompare());

            return this._mapper.Map<IEnumerable<SquadGetListRp>>(diff);
        }

        public async Task UnRegisterMember(int id, int userId)
        {
            var exists = await this._dbContext.Members.Where(c => c.UserId == userId && c.SquadId == id).SingleOrDefaultAsync();
            if (exists != null)
            {                
                this._dbContext.Members.Remove(exists);
                await this._dbContext.SaveChangesAsync();
            }
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

            squad.Update(this._datetimeGateway.GetCurrentDateTime(), createdBy, model.Name, model.Description, model.Avatar, model.Leaders);

            this._dbContext.Update(squad);

            await this._dbContext.SaveChangesAsync();

            return result;
        }
    }
}
