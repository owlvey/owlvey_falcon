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
            IUserIdentityGateway identityGateway, IDateTimeGateway dateTimeGateway, 
            IMapper mapper, ConfigurationComponent configuration) : base(dateTimeGateway, mapper, identityGateway, configuration)
        {
            this._dbContext = dbContext;
        }

        public async Task<BaseComponentResultRp> CreateMember(MemberPostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityGateway.GetIdentity();

            var squad = await this._dbContext.Squads.SingleAsync(c=>c.Id == model.SquadId);
            var user = await this._dbContext.Users.SingleAsync(c => c.Id == model.UserId);

            var entity = MemberEntity.Factory.Create(squad.Id.Value, user.Id.Value, this._datetimeGateway.GetCurrentDateTime(), createdBy);                        
            this._dbContext.Members.Add(entity);
            await this._dbContext.SaveChangesAsync();

            result.AddResult("Id", entity.Id);

            return result;
        }

        public async Task<BaseComponentResultRp> DeleteMember(int memberId)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityGateway.GetIdentity();

            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            var member = await this._dbContext.Members.SingleAsync(c => c.Id == memberId);

            if (member == null) {
                result.AddNotFound($"The Resource {memberId} doesn't exists.");
                return result;
            }

            this._dbContext.Members.Remove(member);
            await this._dbContext.SaveChangesAsync();
            return result;
        }
    }
}
