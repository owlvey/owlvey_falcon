using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Repositories.Features;
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
using Polly;

namespace Owlvey.Falcon.Components
{
    public class FeatureComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;        

        public FeatureComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityService, IDateTimeGateway dateTimeGateway,
            IMapper mapper, ConfigurationComponent configuration ) : base(dateTimeGateway, mapper, identityService, configuration)
        {
            this._dbContext = dbContext;            
            
        }
        

        public async Task<FeatureGetListRp> CreateOrUpdate(CustomerEntity customer,
            string product,
            string name, string description, string avatar)
        {
            var createdBy = this._identityService.GetIdentity();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            var entity = await this._dbContext.Features.Where(c=> c.Product.CustomerId == customer.Id &&
                        c.Product.Name == product &&
                        c.Name == name).SingleOrDefaultAsync();
            if (entity == null)
            {
                var productEntity = await this._dbContext.Products.Where(c => c.CustomerId == customer.Id && c.Name == product).SingleAsync();
                entity = FeatureEntity.Factory.Create(name, this._datetimeGateway.GetCurrentDateTime(), createdBy, productEntity);
            }
            entity.Update(this._datetimeGateway.GetCurrentDateTime(), createdBy, name, avatar, description);
            this._dbContext.Features.Update(entity);
            await this._dbContext.SaveChangesAsync();            
            return this._mapper.Map<FeatureGetListRp>(entity);
        }

        /// <summary>
        /// Create a new Feature
        /// </summary>
        /// <param name="model">Feature Model</param>
        /// <returns></returns>
        public async Task<FeatureGetListRp> CreateFeature(FeaturePostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();

            var retryPolicy = Policy.Handle<DbUpdateException>()
                .WaitAndRetryAsync(this._configuration.DefaultRetryAttempts,
                i => this._configuration.DefaultPauseBetweenFails);

            return await retryPolicy.ExecuteAsync(async () =>
            {
                var entity = await this._dbContext.Features.Where(c => c.ProductId == model.ProductId && c.Name == model.Name).SingleOrDefaultAsync();
                if (entity == null)
                {
                    var product = await this._dbContext.Products.SingleAsync(c => c.Id == model.ProductId);
                    entity = FeatureEntity.Factory.Create(model.Name, this._datetimeGateway.GetCurrentDateTime(), createdBy, product);
                    await this._dbContext.AddAsync(entity);
                    await this._dbContext.SaveChangesAsync();
                }

                return this._mapper.Map<FeatureGetListRp>(entity);
            });
            
        }

        /// <summary>
        /// Delete Feature
        /// </summary>
        /// <param name="key">Feature Id</param>
        /// <returns></returns>
        public async Task DeleteFeature(int id)
        {            
            var modifiedBy = this._identityService.GetIdentity();
            await this._dbContext.RemoveFeature(id);            
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
                model.Description);

            this._dbContext.Features.Update(feature);

            await this._dbContext.SaveChangesAsync();            

            return result;
        }


        #region Features - Squad Relations


        public async Task RegisterSquad(SquadFeaturePostRp model)
        {
            var result = new BaseComponentResultRp();

            var createdBy = this._identityService.GetIdentity();


            var target = await this._dbContext.SquadFeatures.Where(c => c.SquadId == model.SquadId
                                                && c.FeatureId == model.FeatureId).SingleOrDefaultAsync();

            if (target == null) {
                var squad = await this._dbContext.Squads.SingleAsync(c => c.Id == model.SquadId);
                var feature = await this._dbContext.Features.SingleAsync(c => c.Id == model.FeatureId);
                var entity = SquadFeatureEntity.Factory.Create(squad, feature, this._datetimeGateway.GetCurrentDateTime(), createdBy);
                this._dbContext.SquadFeatures.Add(entity);
                await this._dbContext.SaveChangesAsync();
            }            
        }

        public async Task<BaseComponentResultRp> UnRegisterSquad(int squadId, int featureId)
        {
            var result = new BaseComponentResultRp();
            var item = await this._dbContext.SquadFeatures
                .Include(c=>c.Feature).Where(c => c.SquadId == squadId && c.FeatureId == featureId)
                .SingleOrDefaultAsync();
            if (item != null)
            {
                this._dbContext.SquadFeatures.Remove(item);

                await this._dbContext.SaveChangesAsync();
                
            }            

            return result;
        }
        #endregion
    }
}
