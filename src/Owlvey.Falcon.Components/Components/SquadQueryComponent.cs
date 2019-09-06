using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public class SquadQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        public SquadQueryComponent(FalconDbContext dbContext, IDateTimeGateway dateTimeGateway, IMapper mapper,
            IUserIdentityGateway identityService) : base(dateTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// Get Squad by id
        /// </summary>
        /// <param name="key">Squad Id</param>
        /// <returns></returns>
        public async Task<SquadGetRp> GetSquadById(int id)
        {
            var entity = await this._dbContext.Squads.Include(c=>c.Members).ThenInclude(c=>c.User).SingleOrDefaultAsync(c=>c.Id == id);

            if (entity == null)
                return null;

            return this._mapper.Map<SquadGetRp>(entity);
        }

        public async Task<SquadGetRp> GetSquadByName(int customerId, string name)
        {
            var entity = await this._dbContext.Squads.SingleOrDefaultAsync(c => c.Customer.Id == customerId && c.Name == name );

            return this._mapper.Map<SquadGetRp>(entity);
        }

        /// <summary>
        /// Get All Squad
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SquadGetListRp>> GetSquads(int customerId)
        {
            var entities = await this._dbContext.Squads.Where(c=> c.Customer.Id.Equals(customerId)).ToListAsync();

            return this._mapper.Map<IEnumerable<SquadGetListRp>>(entities);
        }
    }
}
