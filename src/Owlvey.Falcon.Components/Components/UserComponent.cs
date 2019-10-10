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

            var target = await this._dbContext.Users.Where(c => c.Id == id).SingleOrDefaultAsync();

            target.Update(model.Email, model.Avatar);

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
        public async Task<BaseComponentResultRp> CreateUser(UserPostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();

            var exists = await this._dbContext.Users.Where(c => c.Email == model.Email).SingleOrDefaultAsync();
            if (exists == null)
            {
                var entity = UserEntity.Factory.Create(createdBy, this._datetimeGateway.GetCurrentDateTime(), model.Email);

                this._dbContext.Users.Add(entity);

                await this._dbContext.SaveChangesAsync();

                result.AddResult("Id", entity.Id);
            }
            else {
                result.AddResult("Id", exists.Id);
            }
            return result;
        }
    }
}
