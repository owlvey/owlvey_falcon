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
        public async Task<IEnumerable<SourceItemGetListRp>> GetBySource(int sourceId)
        {
            var entity = await this._dbContext.SourcesItems.Where(c => c.SourceId == sourceId).ToListAsync();

            return this._mapper.Map<IEnumerable<SourceItemGetListRp>>(entity);
        }
        public async Task<IEnumerable<SourceItemGetListRp>> GetById(int id)
        {
            var entity = await this._dbContext.SourcesItems.Where(c => c.Id == id).ToListAsync();

            return this._mapper.Map<IEnumerable<SourceItemGetListRp>>(entity);
        }
    }
}
