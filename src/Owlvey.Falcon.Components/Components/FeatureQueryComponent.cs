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
    public class FeatureQueryComponent : BaseComponent, IFeatureQueryComponent
    {
        private readonly FalconDbContext _dbContext;
        
        public FeatureQueryComponent(FalconDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// Get Feature by id
        /// </summary>
        /// <param name="key">Feature Id</param>
        /// <returns></returns>
        public async Task<FeatureGetRp> GetFeatureById(int id)
        {
            var entity = await this._dbContext.Features.FirstOrDefaultAsync(c=> c.Id.Equals(id));

            if (entity == null)
                return null;

            return new FeatureGetRp {
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            };
        }

        /// <summary>
        /// Get All Feature
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<FeatureGetListRp>> GetFeatures()
        {
            var entities = await this._dbContext.Features.ToListAsync();

            return entities.Select(entity => new FeatureGetListRp {
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            });
        }
    }
}
