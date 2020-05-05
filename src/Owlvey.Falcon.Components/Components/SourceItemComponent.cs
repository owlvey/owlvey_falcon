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

        public SourceItemComponent(FalconDbContext dbContext, 
            IDateTimeGateway dataTimeGateway, 
            IMapper mapper, 
            IUserIdentityGateway identityService,
            ConfigurationComponent configuration) : base(dataTimeGateway, mapper, identityService, configuration)
        {
            this._dbContext = dbContext;            
        }


        public async Task<IEnumerable<SourceItemGetListRp>> CreateProportion(SourceItemProportionPostRp model) {

            var createdBy = this._identityService.GetIdentity();
            var source = await this._dbContext.Sources.SingleAsync(c => c.Id == model.SourceId);

            var entities = SourceItemEntity.Factory.CreateProportionFromRange(source,model.Start, model.End, model.Proportion,
                this._datetimeGateway.GetCurrentDateTime(), createdBy);

            foreach (var item in entities)
            {
                this._dbContext.SourcesItems.Add(item);
            }
            await this._dbContext.SaveChangesAsync();

            var entitiess = this._dbContext.SourcesItems.Where(c => c.SourceId == source.Id).ToList(); 

            return this._mapper.Map<IEnumerable<SourceItemGetListRp>>(entities);             
        }        
        
        public async Task<IEnumerable<SourceItemGetListRp>> Create(SourceItemInteractionPostRp model)
        {
            
            var createdBy = this._identityService.GetIdentity();
            var on = this._datetimeGateway.GetCurrentDateTime();
            var source = await this._dbContext.Sources.SingleAsync(c => c.Id == model.SourceId);

            var range = SourceItemEntity.Factory.CreateInteractionsFromRange(source, model.Start, model.End, model.Good, model.Total, on, createdBy);

            foreach (var key in model.Clues.Keys)
            {
                foreach (var item in range)
                {
                    ClueEntityFactory.Factory.Create(key, model.Clues[key], this._datetimeGateway.GetCurrentDateTime(), createdBy, item);
                }                
            }

            foreach (var item in range)
            {
                this._dbContext.SourcesItems.Add(item);
            }

            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<IEnumerable<SourceItemGetListRp>>(range);
        }
        public async Task BulkInsert(SourceEntity source, 
            IEnumerable<SourceItemPostRp> sourceItems) {

            var createdBy = this._identityService.GetIdentity();
            int period = 0;
            foreach (var model in sourceItems)
            {
                var interaction = model as SourceItemInteractionPostRp;
                if (interaction != null)
                {
                    var range = SourceItemEntity.Factory.CreateInteractionsFromRange(source, interaction.Start,
                                interaction.End, interaction.Good, interaction.Total,
                                this._datetimeGateway.GetCurrentDateTime(), createdBy);
                    foreach (var clue in model.Clues)
                    {
                        foreach (var item in range)
                        {
                            ClueEntityFactory.Factory.Create(clue.Key, clue.Value,
                            this._datetimeGateway.GetCurrentDateTime(), createdBy, item);
                        }
                    }
                    foreach (var item in range)
                    {
                        this._dbContext.SourcesItems.Add(item);
                        period += 1;
                    }
                }
                var proportion = model as SourceItemProportionPostRp;
                if (proportion != null)
                {
                    var range = SourceItemEntity.Factory.CreateProportionFromRange(source,
                        proportion.Start, proportion.End, proportion.Proportion, 
                                this._datetimeGateway.GetCurrentDateTime(), createdBy);
                    foreach (var clue in model.Clues)
                    {
                        foreach (var item in range)
                        {
                            ClueEntityFactory.Factory.Create(clue.Key, clue.Value,
                            this._datetimeGateway.GetCurrentDateTime(), createdBy, item);
                        }
                    }
                    foreach (var item in range)
                    {
                        this._dbContext.SourcesItems.Add(item);
                        period += 1;
                    }
                }

                if (period > 50) {                    
                    await this._dbContext.SaveChangesAsync();
                    period = 0;
                }
            }
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<SourceItemGetListRp> Update(int sourceItemId,
            int total, int good, DateTime target) {

            var entity = await this._dbContext.SourcesItems.Where(c => c.Id == sourceItemId).SingleAsync();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            entity.Update(total, good, target);
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

        public async Task<BaseComponentResultRp> DeleteSource(int sourceId)
        {
            var result = new BaseComponentResultRp();

            var items = await this._dbContext.SourcesItems.Where(c=> c.SourceId == sourceId).ToListAsync();

            foreach (var item in items)
            {
                this._dbContext.SourcesItems.Remove(item);
            }            

            await this._dbContext.SaveChangesAsync();

            return result;
        }


        public async Task<IEnumerable<SourceItemGetListRp>> GetBySource(int sourceId)
        {
            var entities = await this._dbContext.SourcesItems.Where(c => c.SourceId == sourceId).ToListAsync();

            entities = entities.OrderBy(c => c.Target).ToList();

            return this._mapper.Map<IEnumerable<SourceItemGetListRp>>(entities);
        }
                
        public async Task<IEnumerable<SourceItemGetListRp>> GetAll()
        {
            var entity = await this._dbContext.SourcesItems.ToListAsync();
            return this._mapper.Map<IEnumerable<SourceItemGetListRp>>(entity);
        }
        
        public async Task<SourceItemGetRp> GetById(int id)
        {
            var entity = await this._dbContext.SourcesItems
                .Include(c => c.Clues)
                .Where(c => c.Id == id).SingleAsync();
            return this._mapper.Map<SourceItemGetRp>(entity);
        }

        public async Task<IEnumerable<SourceItemGetListRp>> GetBySourceIdAndDateRange(int sourceId, 
            DateTime start, DateTime end)
        {
            var entity = await this._dbContext.GetSourceItems(sourceId, start, end);
            return this._mapper.Map<IEnumerable<SourceItemGetListRp>>(entity);
        }
        
    }
}
