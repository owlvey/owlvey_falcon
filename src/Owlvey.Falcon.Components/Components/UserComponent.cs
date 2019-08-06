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

        public async Task<BaseComponentResultRp> CreateUser(UserPostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();            

            var entity = UserEntity.Factory.Create(createdBy, this._datetimeGateway.GetCurrentDateTime(), model.Email);

            this._dbContext.Users.Add(entity);

            await this._dbContext.SaveChangesAsync();

            return result;
        }

        public async Task<IEnumerable<UserGetListRp>> GetUsers()
        {
            var entities = await this._dbContext.Users.ToListAsync();

            return this._mapper.Map<IEnumerable<UserGetListRp>>(entities);
            
        }

        public async Task<UserGetRp> GetUserByEmail(string email)
        {
            var entity = await this._dbContext.Users.SingleAsync(c => c.Email == email);
            return this._mapper.Map<UserGetRp>(entity);
        }



    }
}
