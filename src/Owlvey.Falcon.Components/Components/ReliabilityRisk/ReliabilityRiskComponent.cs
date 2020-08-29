using AutoMapper;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Owlvey.Falcon.Components
{
    public class ReliabilityRiskComponent : BaseComponent
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ReliabilityRiskEntity, ReliabilityRiskGetRp>()
                .ForMember(c => c.Source, c => c.MapFrom(d => d.Source != null ? d.Source.Name : ""));
            cfg.CreateMap<ReliabilityThreatEntity, ReliabilityThreatGetRp>();
        }
        private readonly FalconDbContext _dbContext;

        public ReliabilityRiskComponent(FalconDbContext dbContext,
                IUserIdentityGateway identityGateway, IDateTimeGateway dateTimeGateway,
                IMapper mapper, ConfigurationComponent configuration) : base(dateTimeGateway, mapper, identityGateway, configuration)
        {
            this._dbContext = dbContext;
        }


        public async Task<IEnumerable<ReliabilityRiskGetRp>> GetRisks(int? sourceId = null)
        {
            if (sourceId.HasValue)
            {
                var response = await this._dbContext.ReliabilityRisks.Include(c => c.Source)
                    .Where(c => c.SourceId == sourceId).ToListAsync();
                return this._mapper.Map<IEnumerable<ReliabilityRiskGetRp>>(response);
            }
            else
            {
                var response = await this._dbContext.ReliabilityRisks.Include(c => c.Source).ToListAsync();
                return this._mapper.Map<IEnumerable<ReliabilityRiskGetRp>>(response);
            }
        }
        public async Task<ReliabilityRiskGetRp> GetRiskById(int id)
        {
            var response = await this._dbContext.ReliabilityRisks
                .Include(c => c.Source)
                .Where(c => c.Id == id).SingleOrDefaultAsync();
            return this._mapper.Map<ReliabilityRiskGetRp>(response);
        }
        public async Task<ReliabilityRiskGetRp> Create(ReliabilityRiskPostRp model)
        {
            var modifiedBy = this._identityGateway.GetIdentity();
            var modifiedOn = this._datetimeGateway.GetCurrentDateTime();
            var source =await this._dbContext.Sources.Where(c => c.Id == model.SourceId).SingleAsync();
            var item = ReliabilityRiskEntity.Factory.Create(source,
                modifiedOn, modifiedBy, model.Name, model.Avatar,
                model.Reference, model.Description, model.Tags, model.ETTD, model.ETTE,
                model.ETTF, model.UserImpact, model.ETTFail);

            this._dbContext.ReliabilityRisks.Add(item);
            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<ReliabilityRiskGetRp>(item);
        }
        public async Task<ReliabilityRiskGetRp> UpdateRisk(int id,
           ReliabilityRiskPutRp model)
        {            
            var modifiedBy = this._identityGateway.GetIdentity();
            var modifiedOn = this._datetimeGateway.GetCurrentDateTime();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            var item = this._dbContext.ReliabilityRisks.Where(c => c.Id == id).SingleOrDefault();
            if (item != null)
            {
                item.Update(modifiedOn, modifiedBy, model.Name, model.Avatar, model.Reference, model.Description,
                        model.Tags, model.ETTD, model.ETTE, model.ETTF, model.UserImpact, model.ETTFail);                                        
                this._dbContext.ReliabilityRisks.Update(item);
                await this._dbContext.SaveChangesAsync();
            }
            return this._mapper.Map<ReliabilityRiskGetRp>(item);
        }
        public async Task DeleteRisk(int id)
        {
            var item = this._dbContext.ReliabilityRisks.Where(c => c.Id == id).SingleOrDefault();
            if (item != null)
            {
                this._dbContext.ReliabilityRisks.Remove(item);
                await this._dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ReliabilityThreatGetRp>> GetThreats()
        {
            var items = await this._dbContext.ReliabilityThreats.ToListAsync();
            var result = this._mapper.Map<IEnumerable<ReliabilityThreatGetRp>>(items);
            return result;
        }
        public async Task<ReliabilityThreatGetRp> GetThreat(int id)
        {
            var item = await this._dbContext.ReliabilityThreats.Where(c => c.Id == id).SingleAsync();
            return this._mapper.Map<ReliabilityThreatGetRp>(item);
        }
        public async Task<ReliabilityThreatGetRp> CreateThreat(ReliabilityThreatPostRp model)
        {
            var modifiedBy = this._identityGateway.GetIdentity();
            var modifiedOn = this._datetimeGateway.GetCurrentDateTime();
            var item = ReliabilityThreatEntity.Factory.Create(modifiedOn, modifiedBy, model.Name);
            this._dbContext.Add(item);
            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<ReliabilityThreatGetRp>(item);
        }
        public async Task<ReliabilityThreatGetRp> UpdateThreat(int id, ReliabilityThreatPutRp model)
        {
            var modifiedBy = this._identityGateway.GetIdentity();
            var modifiedOn = this._datetimeGateway.GetCurrentDateTime();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            var item = this._dbContext.ReliabilityThreats.Where(c => c.Id == id).SingleOrDefault();
            if (item != null)
            {
                item.Update(modifiedOn, modifiedBy, model.Name, model.Description, model.Tags,
                    model.Reference, model.Tags, model.ETTD, model.ETTE, model.ETTF, model.UserImpact, model.ETTFail);
                this._dbContext.ReliabilityThreats.Update(item);
                await this._dbContext.SaveChangesAsync();
            }
            return this._mapper.Map<ReliabilityThreatGetRp>(item);
        }
        public async Task DeleteThreat(int id)
        {
            var item = this._dbContext.ReliabilityThreats.Where(c => c.Id == id).SingleOrDefault();
            if (item != null)
            {
                this._dbContext.ReliabilityThreats.Remove(item);
                await this._dbContext.SaveChangesAsync();
            }
        }
    }
}
