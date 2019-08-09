using System;
using System.Threading.Tasks;
using AutoMapper;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Components
{
    public class SquadFeatureComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;

        public SquadFeatureComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityService, IDateTimeGateway dateTimeGateway, IMapper mapper) : base(dateTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
        }

        public async Task<SquadFeatureGetRp> GetId(int id)
        {
            var entity = await this._dbContext.SquadFeatures.SingleOrDefaultAsync(c => c.Id.Equals(id));

            if (entity == null)
                return null;

            return this._mapper.Map<SquadFeatureGetRp>(entity);
        }

        public async Task<BaseComponentResultRp> CreateSquadFeature(SquadFeaturePostRp model)
        {
            var result = new BaseComponentResultRp();

            var createdBy = this._identityService.GetIdentity();

            var squad = await this._dbContext.Squads.SingleAsync(c => c.Id == model.SquadId);
            var feature = await this._dbContext.Features.SingleAsync(c => c.Id == model.SquadId);            
            
            var entity = SquadFeatureEntity.Factory.Create(squad, feature, this._datetimeGateway.GetCurrentDateTime(), createdBy);

            this._dbContext.SquadFeatures.Add(entity);

            await this._dbContext.SaveChangesAsync();

            result.AddResult("Id", entity.Id);

            return result;
        }
    }
}
