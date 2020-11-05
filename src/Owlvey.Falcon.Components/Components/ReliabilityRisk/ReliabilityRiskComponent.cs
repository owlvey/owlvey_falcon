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

        public async Task<IEnumerable<ReliabilityThreatGetRp>> CreateDefault(){
            var result = new List<ReliabilityThreatGetRp>();
            var integrationPoints = await this.CreateThreat(new ReliabilityThreatPostRp(){
                Name = "Integration Points"          
            });
            result.Add(await this.UpdateThreat(integrationPoints.Id, new ReliabilityThreatPutRp() { 
                Description = "Integration points are the number-one killer of systems. Every single one presents a stability risk.",
                Avatar = "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcQv1jeFPSj_0NEhrUk3IdzvoeKpBkQ1wPIXfw&usqp=CAU", 
                Tags = "#integration-point",  
                Reference = "https://learning.oreilly.com/library/view/release-it/9781680500264/"  
            }));

            var chain = await this.CreateThreat(new ReliabilityThreatPostRp(){
                Name = "Chain reactions"          
            });
            result.Add(await this.UpdateThreat(chain.Id, new ReliabilityThreatPutRp() { 
                Description = "Whenever one server in a pool goes down, the rest of the servers pick up the slack. The increased load makes them more likely to fail, likely from the same defect.",
                Avatar = "https://cdn4.iconfinder.com/data/icons/security-39/92/icon91-18-512.png", 
                Tags = "#chain-reactions",  
                Reference = "https://learning.oreilly.com/library/view/release-it/9781680500264/"  
            }));

            var failures = await this.CreateThreat(new ReliabilityThreatPostRp(){
                Name = "Cascading Failures"          
            });
            result.Add( await this.UpdateThreat(failures.Id, new ReliabilityThreatPutRp() { 
                Description = "A cascading failure occurs when a crack in one layer triggers a crack in a calling layer",
                Avatar = "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcQdujTrzBkaFf7VZ5n2k8VohH1zmvEoTAN7yw&usqp=CAU",
                Tags = "#cascading-failures",  
                Reference = "https://learning.oreilly.com/library/view/release-it/9781680500264/"  
            }));
            var userMemory = await this.CreateThreat(new ReliabilityThreatPostRp(){
                Name = "User Memory Consumption"          
            });
            result.Add(await this.UpdateThreat(userMemory.Id, new ReliabilityThreatPutRp() { 
                Description = "Each user’s session requires some memory. Minimize that memory to improve your capacity. Use a session only for caching so you can purge the session’s contents if memory gets tight.",
                Avatar = "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcR30SI-uBVs2LI0-oPqsNDVgvVL-ojsD8fiNQ&usqp=CAU",
                Tags = "#memory-consumption",  
                Reference = "https://learning.oreilly.com/library/view/release-it/9781680500264/"  
            }));

            var userMobs = await this.CreateThreat(new ReliabilityThreatPostRp(){
                Name = "User Mobs"          
            });
            result.Add( await this.UpdateThreat(userMobs.Id, new ReliabilityThreatPutRp() { 
                Description = "Large mobs can trigger hangs, deadlocks, and obscure race conditions. Run special stress tests to hammer deep links or hot URLs.",
                Avatar = "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcRfoj5SgTCbm4RUufbzvTriLVHHi-D5-SN6Ng&usqp=CAU",
                Tags = "#user-mobs",  
                Reference = "https://learning.oreilly.com/library/view/release-it/9781680500264/"  
            }));
            var blockedThreads = await this.CreateThreat(new ReliabilityThreatPostRp(){
                Name = "Blocked threads"          
            });
            result.Add(await this.UpdateThreat(blockedThreads.Id, new ReliabilityThreatPutRp() { 
                Description = "Blocked threads end up slowing responses, creating a feedback loop that amplifies a minor problem into total system failure.",
                Avatar = "https://cdn4.iconfinder.com/data/icons/security-39/92/icon91-18-512.png", 
                Tags = "#blocked-threads",  
                Reference = "https://learning.oreilly.com/library/view/release-it/9781680500264/"  
            }));

            var selfDenied = await this.CreateThreat(new ReliabilityThreatPostRp(){
                Name = "Attacks of Self-Denied"          
            });
            result.Add( await this.UpdateThreat(selfDenied.Id, new ReliabilityThreatPutRp() { 
                Description = "Good marketing can kill you at any time.",
                Avatar = "https://cdn4.iconfinder.com/data/icons/security-39/92/icon91-18-512.png", 
                Tags = "#self-denied",  
                Reference = "https://learning.oreilly.com/library/view/release-it/9781680500264/"  
            }));

            var scaling = await this.CreateThreat(new ReliabilityThreatPostRp(){
                Name = "Scaling Effects"          
            });
            result.Add( await this.UpdateThreat(scaling.Id, new ReliabilityThreatPutRp() { 
                Description = "Whenever possible, build out a shared-nothing architecture. Each server operates independently, without the need for coordination or calls to a central service. With a shared-nothing architecture, capacity scales with the number of servers.",
                Avatar = "https://cdn4.iconfinder.com/data/icons/security-39/92/icon91-18-512.png", 
                Tags = "#scaling-effects",  
                Reference = "https://learning.oreilly.com/library/view/release-it/9781680500264/"  
            }));

            var balanced = await this.CreateThreat(new ReliabilityThreatPostRp(){
                Name = "Unbalanced Capacities"          
            });
            
            result.Add(await this.UpdateThreat(balanced.Id, new ReliabilityThreatPutRp() { 
                Description = "Your front-end service experiences an increase in load that’s visible from a dashboard. Adding more servers to the front-end solves the problem. However, each of these front-end servers requires a connection to a back-end service that now is vastly under-provisioned.",
                Avatar = "https://cdn4.iconfinder.com/data/icons/security-39/92/icon91-18-512.png", 
                Tags = "#unbalanced-capacities",  
                Reference = "https://learning.oreilly.com/library/view/release-it/9781680500264/"  
            }));



            var slowResponses = await this.CreateThreat(new ReliabilityThreatPostRp(){
                Name = "Slow Responses"          
            });
            result.Add( await this.UpdateThreat(slowResponses.Id, new ReliabilityThreatPutRp() { 
                Description = "Generating a slow response is worse than returning an error.",
                Avatar = "https://cdn4.iconfinder.com/data/icons/security-39/92/icon91-18-512.png", 
                Tags = "#slow-responses",  
                Reference = "https://learning.oreilly.com/library/view/release-it/9781680500264/"  
            }));


            var slaInversion = await this.CreateThreat(new ReliabilityThreatPostRp(){
                Name = "SLA Inversion"          
            });
            result.Add(await this.UpdateThreat(slaInversion.Id, new ReliabilityThreatPutRp() { 
                Description = "When calling third parties, service levels can only decrease. Your system is only as reliable as the systems it depends on.",
                Avatar = "https://cdn4.iconfinder.com/data/icons/security-39/92/icon91-18-512.png", 
                Tags = "#sla-inversion",  
                Reference = "https://learning.oreilly.com/library/view/release-it/9781680500264/"  
            }));

            var unboundedResultSet = await this.CreateThreat(new ReliabilityThreatPostRp(){
                Name = "Unbounded Result Sets"          
            });
            result.Add( await this.UpdateThreat(unboundedResultSet.Id, new ReliabilityThreatPutRp() { 
                Description = "Be prepared for the possibility that queries for data return infinite results. Typically this will not happen with test data, but after code hits production. Be sure to test with realistic data volumes.", 
                Avatar = "https://cdn4.iconfinder.com/data/icons/security-39/92/icon91-18-512.png", 
                Tags = "#unbounded-results",  
                Reference = "https://learning.oreilly.com/library/view/release-it/9781680500264/"  
            }));
            return result;
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
                item.Update(modifiedOn, modifiedBy, model.Name, model.Avatar, model.Reference,
                    model.Description, model.Tags, model.ETTD, model.ETTE, model.ETTF, 
                    model.UserImpact, model.ETTFail);
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
