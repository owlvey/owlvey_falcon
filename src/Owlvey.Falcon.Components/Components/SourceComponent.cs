﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Owlvey.Falcon.Components
{
    public class SourceComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;

        public SourceComponent(FalconDbContext dbContext, IDateTimeGateway dataTimeGateway, IMapper mapper, IUserIdentityGateway identityService) : base(dataTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;
        }
        public async Task<BaseComponentResultRp> Create(SourcePostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();

            var product = await this._dbContext.Products.SingleAsync(c => c.Id == model.ProductId);
            var entity = SourceEntity.Factory.Create(product, model.Name, this._datetimeGateway.GetCurrentDateTime(), createdBy);
            
            await this._dbContext.AddAsync(entity);

            await this._dbContext.SaveChangesAsync();

            result.AddResult("Id", entity.Id);

            return result;
        }

        public async Task<SourceGetRp> GetByName(int productId, string name)
        {
            var entity = await this._dbContext.Sources.SingleOrDefaultAsync(c => c.Product.Id == productId && c.Name == name);
            return this._mapper.Map<SourceGetRp>(entity);
        }

        public async Task<SourceGetRp> GetById(int id)
        {
            var entity = await this._dbContext.Sources.SingleOrDefaultAsync(c => c.Id == id);
            return this._mapper.Map<SourceGetRp>(entity);
        }

        public async Task<IEnumerable<SourceGetListRp>> GetByProductId(int productId)
        {
            var entities = await this._dbContext.Sources.Where(c => c.Product.Id == productId).ToListAsync();
            return this._mapper.Map<IEnumerable<SourceGetListRp>>(entities);
        }

        public async Task<IEnumerable<SourceGetListRp>> GetByIndicatorId(int indicatorId)
        {
            var entities = await this._dbContext.Indicators.Include(c=>c.Source).Where(c=>c.Id == indicatorId).ToListAsync();
            var targets = entities.Select(c => c.Source).ToList();
            return this._mapper.Map<IEnumerable<SourceGetListRp>>(targets);
        }

    }
}
