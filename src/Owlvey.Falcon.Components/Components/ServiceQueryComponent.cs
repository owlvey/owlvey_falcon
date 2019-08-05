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
    public class ServiceQueryComponent : BaseComponent, IServiceQueryComponent
    {
        private readonly FalconDbContext _dbContext;

        public ServiceQueryComponent(FalconDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// Get Service by id
        /// </summary>
        /// <param name="key">Service Id</param>
        /// <returns></returns>
        public async Task<ServiceGetRp> GetServiceById(int id)
        {
            var entity = await this._dbContext.Services.FirstOrDefaultAsync(c=> c.Id.Equals(id));

            if (entity == null)
                return null;

            return new ServiceGetRp {
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            };
        }

        /// <summary>
        /// Get All Service
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ServiceGetListRp>> GetServices()
        {
            var entities = await this._dbContext.Services.ToListAsync();

            return entities.Select(entity => new ServiceGetListRp {
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            });
        }
    }
}
