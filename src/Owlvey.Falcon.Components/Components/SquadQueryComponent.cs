using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public class SquadQueryComponent : BaseComponent, ISquadQueryComponent
    {
        private readonly FalconDbContext _dbContext;

        public SquadQueryComponent(FalconDbContext dbContext)
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
            var entity = await this._dbContext.Squads.FirstOrDefaultAsync(c=> c.Id.Equals(id));

            if (entity == null)
                return null;

            return new SquadGetRp {
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            };
        }

        /// <summary>
        /// Get All Squad
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SquadGetListRp>> GetSquads()
        {
            var entities = await this._dbContext.Squads.ToListAsync();

            return entities.Select(entity => new SquadGetListRp {
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            });
        }
    }
}
