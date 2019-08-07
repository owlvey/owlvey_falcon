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
    public class FeatureQueryComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;
        
        public FeatureQueryComponent(FalconDbContext dbContext, IDateTimeGateway dateTimeGateway, IMapper mapper, IUserIdentityGateway userIdentityGateway) : base(dateTimeGateway, mapper, userIdentityGateway)
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

            return this._mapper.Map<FeatureGetRp>(entity);
        }


        public async Task<FeatureGetRp> GetFeatureByName(int productId, string name)
        {
            var entity = await this._dbContext.Features.FirstOrDefaultAsync(c => c.Product.Id == productId && c.Name == name);

            if (entity == null)
                return null;

            return this._mapper.Map<FeatureGetRp>(entity);
        }

        /// <summary>
        /// Get All Feature
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<FeatureGetListRp>> GetFeatures()
        {
            var entities = await this._dbContext.Features.ToListAsync();
            return this._mapper.Map<IEnumerable< FeatureGetListRp>>(entities);
        }
    }
}
