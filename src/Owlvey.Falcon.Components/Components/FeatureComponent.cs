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

            var product = await this._dbContext.Products.SingleAsync(c => c.Id == model.ProductId);

            var entity = FeatureEntity.Factory.Create(model.Name, this._datetimeGateway.GetCurrentDateTime(), createdBy, product);
            

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

            var feature = await this._dbContext.Features.SingleAsync(c => c.Id == id);

            if (feature == null)
            {
                result.AddNotFound($"The Resource {id} doesn't exists.");
                return result;
            }

            feature.Name = model.Name ?? feature.Name;
            feature.Avatar = model.Avatar ?? feature.Avatar;
            feature.Update(this._datetimeGateway.GetCurrentDateTime(), createdBy);

            this._dbContext.Features.Update(feature);

            await this._dbContext.SaveChangesAsync();

            return result;
        }
    }
}
