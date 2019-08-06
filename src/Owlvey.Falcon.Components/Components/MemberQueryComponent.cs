using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Owlvey.Falcon.Core.Entities;
using AutoMapper;

namespace Owlvey.Falcon.Components
{
    public class MemberQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;

        public MemberQueryComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityService, IDateTimeGateway dateTimeGateway, IMapper mapper) : base(dateTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<MemberGetListRp>> GetMembers(int squadId)
        {
            var entities = await this._dbContext.Members.Where(c=>c.Squad.Id == squadId).ToListAsync();
            return this._mapper.Map<IEnumerable<MemberGetListRp>>(entities);
        }

    }
}
