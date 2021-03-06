﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Repositories.Sources;
using Owlvey.Falcon.Core;
using Owlvey.Falcon.Core.Validators;
using Polly;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Components
{
    public class IndicatorComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;

        public IndicatorComponent(FalconDbContext dbContext, IDateTimeGateway dataTimeGateway, IMapper mapper, 
            IUserIdentityGateway identityGateway, ConfigurationComponent configuration) : base(dataTimeGateway, mapper, identityGateway, configuration)
        {
            this._dbContext = dbContext;
        }

        public async Task<BaseComponentResultRp> Delete(int indicatorId) {
            var result = new BaseComponentResultRp();
            var entity = await this._dbContext.Indicators.Where(c => c.Id == indicatorId).SingleOrDefaultAsync();
            if (entity != null) {
                this._dbContext.Indicators.Remove(entity);
                await this._dbContext.SaveChangesAsync();
            }
            return result;
        }

        public async Task<BaseComponentResultRp> Delete(int featureId, int sourceId)
        {
            var result = new BaseComponentResultRp();
            var entity = await this._dbContext.Indicators.Where(c => c.FeatureId == featureId && c.SourceId == sourceId).SingleOrDefaultAsync();
            if (entity != null)
            {
                this._dbContext.Indicators.Remove(entity);
                await this._dbContext.SaveChangesAsync();
            }
            return result;
        }

        public async Task<IndicatorGetListRp>  Create(int customerId, string product, string source, string feature)
        {
            var createdBy = this._identityGateway.GetIdentity();
            var productEntity = await this._dbContext.Products.Where(c => c.CustomerId == customerId && c.Name == product).SingleAsync();
            var sourceEntity = NotNullValidator.Validate(await this._dbContext.GetSource(productEntity.Id.Value, source), c=>c.Name, source);
            var featureEntity = await this._dbContext.Features.Where(c => c.ProductId == productEntity.Id && c.Name == feature).SingleAsync();

            var sli = await this._dbContext.Indicators.Where(c => 
                            c.FeatureId == featureEntity.Id && c.SourceId == sourceEntity.Id).SingleOrDefaultAsync();
            if (sli == null) {
                sli = IndicatorEntity.Factory.Create(featureEntity, sourceEntity,
                    this._datetimeGateway.GetCurrentDateTime(), createdBy);
                this._dbContext.Indicators.Add(sli);
                await this._dbContext.SaveChangesAsync();
            }
            return this._mapper.Map<IndicatorGetListRp>(sli);
        }

        public async Task<IndicatorGetListRp> Create(int featureId, int sourceId) {
            var createdBy = this._identityGateway.GetIdentity();
            var updatedOn = this._datetimeGateway.GetCurrentDateTime();

            var feature = await this._dbContext.Features.Where(c => c.Id == featureId).SingleAsync();
            var source = await this._dbContext.Sources.Where(c => c.Id == sourceId).SingleAsync();

            var retryPolicy = Policy.Handle<DbUpdateException>()
                .WaitAndRetryAsync(this._configuration.DefaultRetryAttempts,
                i => this._configuration.DefaultPauseBetweenFails);

            return await retryPolicy.ExecuteAsync(async () =>
            {
                var sli = await this._dbContext.Indicators.Where(c => 
                            c.FeatureId == featureId && c.SourceId == sourceId).SingleOrDefaultAsync();
                if (sli == null)
                {                    
                    sli = IndicatorEntity.Factory.Create(feature, source, updatedOn, createdBy);
                    await this._dbContext.Indicators.AddAsync(sli);
                    await this._dbContext.SaveChangesAsync();
                }
                return this._mapper.Map<IndicatorGetListRp>(sli);
            });
        }     


        public async Task<IEnumerable<SourceGetListRp>> GetSourcesComplement(int featureId)
        {
            IEnumerable<SourceGetListRp> result = new List<SourceGetListRp>();
            var feature = await this._dbContext.Features.SingleAsync(c => c.Id == featureId);
            
            var sources = await this._dbContext.Sources.Where(c=>c.ProductId == feature.ProductId).ToListAsync();
            
            var existing = await this._dbContext.Indicators.Include(c => c.Source)
                .Where(c => c.FeatureId == featureId).Select(c => c.Source).ToListAsync();
            result = this._mapper.Map<IEnumerable<SourceGetListRp>>(sources.Except(existing, new SourceEntityComparer()));            
            return result;
        }

        public async Task<IEnumerable<IndicatorGetListRp>> GetByFeature(int featureId)
        {
            var entity = await this._dbContext.Indicators.Include(c=>c.Feature).Include(c=>c.Source).Where(c => c.Feature.Id == featureId).ToListAsync();
            return this._mapper.Map<IEnumerable<IndicatorGetListRp>>(entity);
        }

       

        public async Task<IndicatorGetRp> GetById(int id)
        {
            var entity = await this._dbContext.Indicators.Include(c=>c.Feature).Include(c=> c.Source).SingleOrDefaultAsync(c => c.Id == id);

            return this._mapper.Map<IndicatorGetRp>(entity);
        }  
    }
}
