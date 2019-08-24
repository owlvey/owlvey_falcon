using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Owlvey.Falcon.Components
{
    public class FeatureComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;        

        public FeatureComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityService, IDateTimeGateway dateTimeGateway, IMapper mapper) : base(dateTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
            
        }

        /// <summary>
        /// Create a new Feature
        /// </summary>
        /// <param name="model">Feature Model</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> CreateFeature(FeaturePostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();

            var product = await this._dbContext.Products.Include(c=> c.Features).SingleAsync(c => c.Id == model.ProductId);

            // Validate if the resource exists.
            if (product.Features.Any(c => c.Name.Equals(model.Name)))
            {
                result.AddConflict($"The Resource {model.Name} has already been taken.");
                return result;
            }
            
            var entity = FeatureEntity.Factory.Create(model.Name, model.Description, this._datetimeGateway.GetCurrentDateTime(), createdBy, product);
            
            this._dbContext.Add(entity);

            await this._dbContext.SaveChangesAsync();

            result.AddResult("Id", entity.Id);

            return result;
        }

        /// <summary>
        /// Delete Feature
        /// </summary>
        /// <param name="key">Feature Id</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> DeleteFeature(int id)
        {
            var result = new BaseComponentResultRp();
            var modifiedBy = this._identityService.GetIdentity();

            var feature = await this._dbContext.Features.SingleAsync(c => c.Id == id);

            if (feature == null)
            {
                result.AddNotFound($"The Resource {id} doesn't exists.");
                return result;
            }

            feature.Delete(this._datetimeGateway.GetCurrentDateTime(), modifiedBy);

            this._dbContext.Features.Remove(feature);

            await this._dbContext.SaveChangesAsync();

            return result;
        }
        
        /// <summary>
        /// Update Feature
        /// </summary>
        /// <param name="model">Feature Model</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> UpdateFeature(int id, FeaturePutRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();

            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            var feature = await this._dbContext.Features.Include(c=> c.Product).SingleAsync(c => c.Id == id);

            if (feature == null)
            {
                result.AddNotFound($"The Resource {id} doesn't exists.");
                return result;
            }

            // Validate if the resource exists.
            if (!feature.Name.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                var product = await this._dbContext.Products.Include(c => c.Features).SingleAsync(c => c.Id.Equals(feature.Product.Id));

                if (product.Features.Any(c => c.Name.Equals(model.Name)))
                {
                    result.AddConflict($"The Resource {model.Name} has already been taken.");
                    return result;
                }
            }
                                    
            feature.Update(this._datetimeGateway.GetCurrentDateTime(),
                createdBy, model.Name,
                model.Avatar,
                model.Description,
                model.MTTD,
                model.MTTR);

            this._dbContext.Features.Update(feature);

            await this._dbContext.SaveChangesAsync();

            return result;
        }
    }
}
