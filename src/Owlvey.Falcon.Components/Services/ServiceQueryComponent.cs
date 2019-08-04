using Owlvey.Falcon.Components.Interfaces;
using Owlvey.Falcon.Components.Models;
using Owlvey.Falcon.Components.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components.Services
{
    public class ServiceQueryComponent : BaseComponent, IServiceQueryComponent
    {
        private readonly IServiceRepository _serviceRepository;
        public ServiceQueryComponent(IServiceRepository serviceRepository)
        {
            this._serviceRepository = serviceRepository;
        }

        /// <summary>
        /// Get Service by id
        /// </summary>
        /// <param name="key">Service Id</param>
        /// <returns></returns>
        public async Task<ServiceGetRp> GetServiceById(int id)
        {
            var entity = await this._serviceRepository.FindFirst(c=> c.Id.Equals(id));

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
            var entities = await this._serviceRepository.GetAll();

            return entities.Select(entity => new ServiceGetListRp {
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            });
        }
    }
}
