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
    public class SquadQueryComponent : BaseComponent, ISquadQueryComponent
    {
        private readonly ISquadRepository _squadRepository;
        public SquadQueryComponent(ISquadRepository squadRepository)
        {
            this._squadRepository = squadRepository;
        }

        /// <summary>
        /// Get Squad by id
        /// </summary>
        /// <param name="key">Squad Id</param>
        /// <returns></returns>
        public async Task<SquadGetRp> GetSquadById(int id)
        {
            var entity = await this._squadRepository.FindFirst(c=> c.Id.Equals(id));

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
            var entities = await this._squadRepository.GetAll();

            return entities.Select(entity => new SquadGetListRp {
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            });
        }
    }
}
