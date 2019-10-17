using AutoMapper;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Owlvey.Falcon.Core.Entities;
using System.Linq;

namespace Owlvey.Falcon.Components
{
    public class UserComponent: BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        public UserComponent(FalconDbContext dbContext, IDateTimeGateway dateTimeGateway, IMapper mapper,
            IUserIdentityGateway identityService) : base(dateTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
        }


        public async Task<UserGetListRp> PutUser(int id, UserPutRp model) {

            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            var target = await this._dbContext.Users.Where(c => c.Id == id).SingleAsync();
            target.Update(model.Email, model.Avatar, model.Name);
            this._dbContext.Users.Update(target);
            await this._dbContext.SaveChangesAsync();

            return this._mapper.Map<UserGetListRp>(target);

        }
        public async Task DeleteUser(int id) {
            var target = await this._dbContext.Users.Where(c => c.Id == id).SingleOrDefaultAsync();
            if (target != null) {
                this._dbContext.Users.Remove(target);
                await this._dbContext.SaveChangesAsync();
            }
        }

        public async Task<UserGetListRp> CreateOrUpdate(string email, string name, string avatar) {
            var createdBy = this._identityService.GetIdentity();
            var entity = await this._dbContext.Users.Where(c => c.Email == email).SingleOrDefaultAsync();
            if (entity == null)
            {
                entity = UserEntity.Factory.Create(createdBy, this._datetimeGateway.GetCurrentDateTime(), email);
                this._dbContext.Users.Add(entity);
                await this._dbContext.SaveChangesAsync();
                
            }
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            entity.Update(email, name, avatar);
            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<UserGetListRp>(entity);
        }

        public async Task<UserGetListRp> CreateUser(UserPostRp model)
        {            
            var createdBy = this._identityService.GetIdentity();

            var entity = await this._dbContext.Users.Where(c => c.Email == model.Email).SingleOrDefaultAsync();
            if (entity == null)
            {
                entity = UserEntity.Factory.Create(createdBy, this._datetimeGateway.GetCurrentDateTime(), model.Email);
                this._dbContext.Users.Add(entity);
                await this._dbContext.SaveChangesAsync();
            }            
            return this._mapper.Map<UserGetListRp>(entity);
        }
    }
}
