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

namespace Owlvey.Falcon.Components
{
    public class IndicatorComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;

        public IndicatorComponent(FalconDbContext dbContext, IDateTimeGateway dataTimeGateway, IMapper mapper, IUserIdentityGateway identityService) : base(dataTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
        }

        public async Task<BaseComponentResultRp> Create(IndicatorPostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();
            var source = await this._dbContext.Sources.SingleAsync(c => c.Id == model.SourceId);
            var feature = await this._dbContext.Features.SingleAsync(c => c.Id == model.FeatureId);
            var entity = IndicatorEntity.Factory.Create(feature, source, this._datetimeGateway.GetCurrentDateTime(), createdBy);
            
            await this._dbContext.AddAsync(entity);
            await this._dbContext.SaveChangesAsync();

            result.AddResult("Id", entity.Id);

            return result;
        }

        public async Task<IEnumerable<IndicatorGetRp>> GetByFeature(int featureId)
        {
            var entity = await this._dbContext.Indicators.Where(c => c.Feature.Id == featureId).ToListAsync();
            return this._mapper.Map<IEnumerable<IndicatorGetRp>>(entity);
        }
    }
}