using System;
using System.Threading.Tasks;
using AutoMapper;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Core.Entities;
using System.Collections.Generic;
using System.Diagnostics;
using Polly;
//
namespace Owlvey.Falcon.Components
{
    public class JourneyMapComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;

        public JourneyMapComponent(FalconDbContext dbContext, IDateTimeGateway dataTimeGateway, IMapper mapper, IUserIdentityGateway identityGateway,
            ConfigurationComponent configuration) : base(dataTimeGateway, mapper, identityGateway, configuration)
        {
            this._dbContext = dbContext;
        }

        public async Task<BaseComponentResultRp> DeleteMap(int journeyId, int featureId) {
            var result = new BaseComponentResultRp();
            var entity = await this._dbContext.JourneyMaps.Where(c => c.JourneyId == journeyId && c.FeatureId == featureId ).SingleOrDefaultAsync();
            if (entity != null) {
                this._dbContext.JourneyMaps.Remove(entity);
                await this._dbContext.SaveChangesAsync();
            }            
            return result;
        }

        public async Task CreateMap(int customerId, string product, string journey, string feature ) {
            var productEntity = await this._dbContext.Products.Where(c => c.CustomerId == customerId && c.Name == product).SingleAsync();
            var journeyEntity = await this._dbContext.Journeys.Where(c => c.ProductId == productEntity.Id && c.Name == journey).SingleAsync();
            var featureEntity = await this._dbContext.Features.Where(c => c.ProductId == productEntity.Id && c.Name == feature).SingleAsync();
            await this.CreateMap(new JourneyMapPostRp()
            {
                FeatureId = featureEntity.Id,
                JourneyId = journeyEntity.Id
            });           
        }

        public async Task CreateMap(JourneyMapPostRp model)
        {            
            var createdBy = this._identityGateway.GetIdentity();

            var retryPolicy = Policy.Handle<DbUpdateException>()
                .WaitAndRetryAsync(this._configuration.DefaultRetryAttempts,
                i => this._configuration.DefaultPauseBetweenFails);

            await retryPolicy.ExecuteAsync(async () =>
            {                
                var entity = await this._dbContext.JourneyMaps.SingleOrDefaultAsync(c => c.JourneyId == model.JourneyId &&
                            c.FeatureId == model.FeatureId);

                if (entity == null)
                {
                    var journey = await this._dbContext.Journeys.SingleAsync(c => c.Id == model.JourneyId);
                    var feature = await this._dbContext.Features.SingleAsync(c => c.Id == model.FeatureId);
                    entity = JourneyMapEntity.Factory.Create(journey, feature, this._datetimeGateway.GetCurrentDateTime(), createdBy);
                    await this._dbContext.AddAsync(entity);
                    await this._dbContext.SaveChangesAsync();
                }                
            });
        }
        public async Task<IEnumerable<JourneyGetListRp>> GetMaps(int journeyId)
        {
            var entities = await this._dbContext.JourneyMaps.Where(c => c.Journey.Id == journeyId).Select(c => c.Journey).ToListAsync();  
            return this._mapper.Map<IEnumerable<JourneyGetListRp>>(entities);
        }
    }
}
