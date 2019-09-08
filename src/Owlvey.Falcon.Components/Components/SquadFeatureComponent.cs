using System;
using System.Threading.Tasks;
using AutoMapper;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Owlvey.Falcon.Core.Entities;
using System.Collections.Generic;

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

        public async Task<IEnumerable<SquadFeatureGetListRp>> GetAll(){
            var entities = await this._dbContext.SquadFeatures.ToListAsync();
            return this._mapper.Map<IEnumerable<SquadFeatureGetListRp>>(entities);
        }

      
    }
}
