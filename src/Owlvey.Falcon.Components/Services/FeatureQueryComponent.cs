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
    public class FeatureQueryComponent : BaseComponent, IFeatureQueryComponent
    {
        private readonly IFeatureRepository _featureRepository;
        public FeatureQueryComponent(IFeatureRepository featureRepository)
        {
            this._featureRepository = featureRepository;
        }

        /// <summary>
        /// Get Feature by id
        /// </summary>
        /// <param name="key">Feature Id</param>
        /// <returns></returns>
        public async Task<FeatureGetRp> GetFeatureById(int id)
        {
            var entity = await this._featureRepository.FindFirst(c=> c.Id.Equals(id));

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
            var entities = await this._featureRepository.GetAll();

            return entities.Select(entity => new FeatureGetListRp {
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            });
        }
    }
}
