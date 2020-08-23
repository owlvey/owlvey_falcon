using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public class SecurityRiskComponent: BaseComponent
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SecurityRiskEntity, SecurityRiskGetRp>();
            cfg.CreateMap<SecurityThreatEntity, SecurityThreatGetRp>();
            
        }

        private readonly FalconDbContext _dbContext;

        public SecurityRiskComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityService, IDateTimeGateway dateTimeGateway,
            IMapper mapper, ConfigurationComponent configuration) : base(dateTimeGateway, mapper, identityService, configuration)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<SecurityRiskGetRp>> GetRisks(int? sourceId) {
            if (sourceId.HasValue)
            {
                var response = await this._dbContext.SecurityRisks.Where(c => c.SourceId == sourceId).ToListAsync();
                return this._mapper.Map<IEnumerable<SecurityRiskGetRp>>(response);
            }
            else {
                var response = await this._dbContext.SecurityRisks.ToListAsync();
                return this._mapper.Map<IEnumerable<SecurityRiskGetRp>>(response);
            }
            
        }
        public async Task<SecurityRiskGetRp> GetRiskById(int id)
        {
            var response = await this._dbContext.SecurityRisks.Where(c => c.Id == id).SingleOrDefaultAsync();
            return this._mapper.Map<SecurityRiskGetRp>(response);
        }
        public async Task<SecurityRiskGetRp> Create(SecurityRiskPost model)
        {
            var modifiedBy = this._identityService.GetIdentity();
            var modifiedOn = this._datetimeGateway.GetCurrentDateTime();
            var item = SecurityRiskEntity.Factory.Create(
                model.SourceId, 
                model.ThreatId, 
                modifiedBy, 
                modifiedOn);

            this._dbContext.SecurityRisks.Add(item);
            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<SecurityRiskGetRp>(item);
        }
        public async Task<SecurityRiskGetRp> UpdateRisk(int id, SecurityRiskPut model)
        {
            var modifiedBy = this._identityService.GetIdentity();
            var modifiedOn = this._datetimeGateway.GetCurrentDateTime();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            var item = this._dbContext.SecurityRisks.Where(c => c.Id == id).SingleOrDefault();
            if (item != null) {

                item.Update(modifiedOn, modifiedBy, model.AgentSkillLevel);
                await this._dbContext.SaveChangesAsync();
            }
            return this._mapper.Map<SecurityRiskGetRp>(item);
        }
        public async Task DeleteRisk(int id)
        {
            var item = this._dbContext.SecurityRisks.Where(c => c.Id == id).SingleOrDefault();
            if (item != null)
            {
                this._dbContext.SecurityRisks.Remove(item);
                await this._dbContext.SaveChangesAsync();
            }            
        }

        public async Task<IEnumerable<SecurityThreatGetRp>> GetThreats()
        {
            var items = await  this._dbContext.SecurityThreats.ToListAsync();
            var result = this._mapper.Map<IEnumerable<SecurityThreatGetRp>>(items);
            return result;
        }
        public async Task<SecurityThreatGetRp> GetThreat(int id)
        {
            var item = await this._dbContext.SecurityThreats.Where(c => c.Id == id).SingleOrDefaultAsync();
            return this._mapper.Map<SecurityThreatGetRp>(item);
        }
        public async Task<SecurityThreatGetRp> CreateThreat(SecurityThreatPostRp model) {
            var modifiedBy = this._identityService.GetIdentity();
            var modifiedOn = this._datetimeGateway.GetCurrentDateTime();
            var item = SecurityThreatEntity.Create(model.Name, modifiedBy, modifiedOn);
            this._dbContext.Add(item);
            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<SecurityThreatGetRp>(item);
        }
        public async Task<SecurityThreatGetRp> UpdateThreat(int id, SecurityThreatPutRp model)
        {
            var modifiedBy = this._identityService.GetIdentity();
            var modifiedOn = this._datetimeGateway.GetCurrentDateTime();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            var item = this._dbContext.SecurityThreats.Where(c => c.Id == id).SingleOrDefault();
            if (item != null)
            {
                item.Update(modifiedOn, modifiedBy, model.Name);
                await this._dbContext.SaveChangesAsync();
            }
            return this._mapper.Map<SecurityThreatGetRp>(item);
        }
        public async Task DeleteThreat(int id)
        {
            var item = this._dbContext.SecurityThreats.Where(c => c.Id == id).SingleOrDefault();
            if (item != null)
            {
                this._dbContext.SecurityThreats.Remove(item);
                await this._dbContext.SaveChangesAsync();
            }
        }
    }
}
