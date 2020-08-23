using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Owlvey.Falcon.Repositories.Journeys;
using Polly;

namespace Owlvey.Falcon.Components
{
    public class JourneyComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;    

        public JourneyComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityGateway, IDateTimeGateway dateTimeGateway,
            IMapper mapper, ConfigurationComponent configuration) : base(dateTimeGateway, mapper, 
                identityGateway, configuration)
        {
            this._dbContext = dbContext;                        
        }

        public async Task<JourneyGetListRp> CreateOrUpdate(CustomerEntity customer,
            string product, string name, string description, string avatar, 
            decimal availabilitySlo,
            decimal latencySlo,
            decimal experienceSlo,
            SLAValue slaValue,
            string leaders,
            string group)
        {
            var createdBy = this._identityGateway.GetIdentity();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            var entity = await this._dbContext.Journeys.Where(c => c.Product.CustomerId == customer.Id
                                     && c.Product.Name == product && c.Name == name).SingleOrDefaultAsync();
            if (entity == null)
            {
                var productEntity = await this._dbContext.Products.Where(c => c.CustomerId == customer.Id && c.Name == product).SingleAsync();
                entity = JourneyEntity.Factory.Create(name, this._datetimeGateway.GetCurrentDateTime(), createdBy, productEntity);
            }

           
            entity.Update(this._datetimeGateway.GetCurrentDateTime(), createdBy, 
                name, availabilitySlo, latencySlo, experienceSlo, slaValue, description, avatar, leaders, group);

            this._dbContext.Journeys.Update(entity);
            await this._dbContext.SaveChangesAsync();

            return this._mapper.Map<JourneyGetListRp>(entity);
        }

        /// <summary>
        /// Create a new journey
        /// </summary>
        /// <param name="model">journey Model</param>
        /// <returns></returns>
        public async Task<JourneyGetListRp> Create(JourneyPostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityGateway.GetIdentity();

            var retryPolicy = Policy.Handle<DbUpdateException>()
                .WaitAndRetryAsync(this._configuration.DefaultRetryAttempts,
                i => this._configuration.DefaultPauseBetweenFails);

            return await retryPolicy.ExecuteAsync(async () =>
            {
                var product = await this._dbContext.Products.Where(c => c.Id == model.ProductId).SingleAsync();
                var entity = await this._dbContext.Journeys.Where(c => c.ProductId == model.ProductId && c.Name == model.Name).SingleOrDefaultAsync();                
                if (entity == null)
                {
                    entity = JourneyEntity.Factory.Create(model.Name, this._datetimeGateway.GetCurrentDateTime(),
                        createdBy, product);
                    this._dbContext.Journeys.Add(entity);
                    await this._dbContext.SaveChangesAsyncRetry<DbUpdateException>();
                }                                
                return this._mapper.Map<JourneyGetListRp>(entity);
            });
                        
        }

        /// <summary>
        /// Delete journey
        /// </summary>
        /// <param name="key">journey Id</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> Delete(int id)
        {
            var result = new BaseComponentResultRp();
            var modifiedBy = this._identityGateway.GetIdentity();

            await this._dbContext.RemoveJourney(id);                        

            return result;
        }

        /// <summary>
        /// Update journey
        /// </summary>
        /// <param name="model">journey Model</param>
        /// <returns></returns>
        public async Task<BaseComponentResultRp> Update(int id, JourneyPutRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityGateway.GetIdentity();

            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            var journey = await this._dbContext.Journeys.Include(c => c.Product).SingleAsync(c => c.Id == id);

            if (journey == null)
            {
                result.AddNotFound($"The Resource {id} doesn't exists.");
                return result;
            }
        
            journey.Update(this._datetimeGateway.GetCurrentDateTime(), createdBy, model.Name, 
                model.AvailabilitySlo,
                model.LatencySlo,
                model.ExperienceSlo,
                new SLAValue(model.AvailabilitySLA, model.LatencySLA),
                model.Description, 
                model.Avatar, model.Leaders, model.Group);

            this._dbContext.Journeys.Update(journey);

            await this._dbContext.SaveChangesAsync();            

            return result;
        }
    }
}
