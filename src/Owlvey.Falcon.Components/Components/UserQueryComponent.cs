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
    public class UserQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        public UserQueryComponent(FalconDbContext dbContext, IDateTimeGateway dateTimeGateway, IMapper mapper,
            IUserIdentityGateway identityService) : base(dateTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
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

        public async Task<UserGetRp> GetUserById(int id)
        {
            var entity = await this._dbContext.Users.SingleOrDefaultAsync(c => c.Id == id);
            return this._mapper.Map<UserGetRp>(entity);
        }

    }
}
