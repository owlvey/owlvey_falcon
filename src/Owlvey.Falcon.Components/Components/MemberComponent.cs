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
    public class MemberComponent: BaseComponent
    {
        private readonly FalconDbContext _dbContext;

        public MemberComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityService, IDateTimeGateway dateTimeGateway, IMapper mapper) : base(dateTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
        }

        public async Task<BaseComponentResultRp> CreateMember(MemberPostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();

            var squad = await this._dbContext.Squads.SingleAsync(c=>c.Id == model.SquadId);
            var user = await this._dbContext.Users.SingleAsync(c => c.Id == model.UserId);

            var entity = MemberEntity.Factory.Create(squad, user, this._datetimeGateway.GetCurrentDateTime(), createdBy);                        
            this._dbContext.Members.Add(entity);
            await this._dbContext.SaveChangesAsync();
            return result;
        }
        public async Task<IEnumerable<MemberGetListRp>> GetMembers(int squadId)
        {
            var entities = await this._dbContext.Members.Where(c=>c.Squad.Id == squadId).ToListAsync();
            return this._mapper.Map<IEnumerable<MemberGetListRp>>(entities);
        }

    }
}
