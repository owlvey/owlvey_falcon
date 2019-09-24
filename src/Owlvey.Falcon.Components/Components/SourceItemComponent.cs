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
using Owlvey.Falcon.Core;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Components
{
    public class SourceItemComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;

        public SourceItemComponent(FalconDbContext dbContext, IDateTimeGateway dataTimeGateway, IMapper mapper, IUserIdentityGateway identityService) : base(dataTimeGateway, mapper, identityService)
        {
            this._dbContext = dbContext;            
        }        
        public async Task<BaseComponentResultRp> Create(SourceItemPostRp model)
        {
            var result = new BaseComponentResultRp();
            var createdBy = this._identityService.GetIdentity();
            var source = await this._dbContext.Sources.SingleAsync(c => c.Id == model.SourceId);
            var entity = SourceItemEntity.Factory.Create(source, model.Start, model.End, model.Good, model.Total, this._datetimeGateway.GetCurrentDateTime(), createdBy);
            this._dbContext.SourcesItems.Add(entity);
            await this._dbContext.SaveChangesAsync();
            result.AddResult("Id", entity.Id);
            return result;
        }
        public async Task<SourceItemGetListRp> Update(int sourceItemId,
            int total, int good, DateTime start, DateTime end ) {

            var entity = await this._dbContext.SourcesItems.Where(c => c.Id == sourceItemId).SingleAsync();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            entity.Update(total, good, start, end);
            this._dbContext.SourcesItems.Update(entity);
            await this._dbContext.SaveChangesAsync();

            return this._mapper.Map<SourceItemGetListRp>(entity);            
        }

        public async Task<BaseComponentResultRp> Delete(int id)
        {
            var result = new BaseComponentResultRp();
            
            var source = await this._dbContext.SourcesItems.SingleAsync(c => c.Id == id);

            this._dbContext.SourcesItems.Remove(source);

            await this._dbContext.SaveChangesAsync();
            
            return result;
        }

        public async Task<IEnumerable<SourceItemGetListRp>> GetBySource(int sourceId)
        {
            var entity = await this._dbContext.SourcesItems.Where(c => c.SourceId == sourceId).OrderBy(c=>c.Start).ToListAsync();

            return this._mapper.Map<IEnumerable<SourceItemGetListRp>>(entity);
        }
                
        public async Task<IEnumerable<SourceItemGetListRp>> GetAll()
        {
            var entity = await this._dbContext.SourcesItems.ToListAsync();
            return this._mapper.Map<IEnumerable<SourceItemGetListRp>>(entity);
        }

      
    
        public async Task<IEnumerable<SourceItemGetListRp>> GetById(int id)
        {
            var entity = await this._dbContext.SourcesItems.Where(c => c.Id == id).ToListAsync();

            return this._mapper.Map<IEnumerable<SourceItemGetListRp>>(entity);
        }

        public IEnumerable<SourceItemGetListRp> GetBySourceIdAndDateRange(int sourceId, DateTime start, DateTime end)
        {
            var entity = this._dbContext.GetSourceItems(sourceId, start, end);
            return this._mapper.Map<IEnumerable<SourceItemGetListRp>>(entity);
        }
    }
}
