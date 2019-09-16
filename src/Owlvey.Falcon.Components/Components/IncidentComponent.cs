using AutoMapper;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Components
{
    public class IncidentComponent : BaseComponent
    {
        private readonly FalconDbContext _dbContext;

        public IncidentComponent(FalconDbContext dbContext, IMapper mapper,
            IDateTimeGateway dateTimeGateway, IUserIdentityGateway identityService) : base(dateTimeGateway, mapper, identityService)

        {
            this._dbContext = dbContext;                       
        }
        public async Task<IEnumerable<IncidentGetListRp>> Get(int productId, PeriodValue period) {
            var incidents = await this._dbContext.Incidents
                .Where(c => c.ProductId == productId && ( c.ModifiedOn >= period.Start && c.ModifiedOn <= period.End) ).ToListAsync();
            return this._mapper.Map<IEnumerable<IncidentGetListRp>>(incidents);
        }

        public async Task<IncidentDetailtRp> Get(int id)
        {
            var incident = await this._dbContext.Incidents
                .Include(c=>c.FeatureMaps).ThenInclude(c=>c.Feature)
                .Where(c => c.Id == id).SingleOrDefaultAsync();
            var tmp = this._mapper.Map<IncidentDetailtRp>(incident);
            
            return tmp;
        }

        public async Task<IEnumerable<FeatureLiteRp>> GetFeatureComplement(int id)
        {
            var incident = await this._dbContext.Incidents.Include(c=>c.FeatureMaps).ThenInclude(c=>c.Feature).Where(c => c.Id == id).SingleAsync();

            var features = await this._dbContext.Features.Where(c=>c.ProductId == incident.ProductId).ToListAsync();

            var complement = features.Except(incident.FeatureMaps.Select(c => c.Feature).ToList(), new FeatureCompare());

            return this._mapper.Map<IEnumerable<FeatureLiteRp>>(complement);
        }



        public async Task<IncidentGetListRp> Post(IncidentPostRp model) {
            var createdBy = this._identityService.GetIdentity();
            var product = await this._dbContext.Products.Where(c => c.Id == model.ProductId).SingleAsync();
            var entity = IncidentEntity.Factory.Create(model.Title, this._datetimeGateway.GetCurrentDateTime(), createdBy, product);
            this._dbContext.Incidents.Add(entity);
            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<IncidentGetListRp>(entity);
        }

        public async Task<IncidentGetListRp> Put(IncidentPutRp model) {
            var createdBy = this._identityService.GetIdentity();
            var incident = await this._dbContext.Incidents.Where(c => c.Id == model.Id).SingleAsync();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            incident.Update(model.Title, model.Description, createdBy, this._datetimeGateway.GetCurrentDateTime(),
                 model.Start,
                 model.TTD, model.TTE, model.TTF, model.URL);
            this._dbContext.Incidents.Update(incident);
            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<IncidentGetListRp>(incident);
        }

        public async Task RegisterFeature(int incidentId, int featureId) {
            var createdBy = this._identityService.GetIdentity();
            var entity = await this._dbContext.IncidentMaps.Where(c => c.IncidentId == incidentId && c.FeatureId == featureId).SingleOrDefaultAsync();

            if (entity == null) {

                var feature = await  this._dbContext.Features.Where(c => c.Id == featureId).SingleAsync();
                var incident = await this._dbContext.Incidents.Where(c => c.Id == incidentId).SingleAsync();

                entity = IncidentMapEntity.Factory.Create(this._datetimeGateway.GetCurrentDateTime(), createdBy, feature, incident);

                this._dbContext.IncidentMaps.Add(entity);
                await this._dbContext.SaveChangesAsync();
            }
        }

        public async Task UnRegisterFeature(int incidentId, int featureId)
        {
            var createdBy = this._identityService.GetIdentity();
            var entity = await this._dbContext.IncidentMaps.Where(c => c.IncidentId == incidentId && c.FeatureId == featureId).SingleOrDefaultAsync();
            if (entity != null)
            {   
                this._dbContext.IncidentMaps.Remove(entity);
                await this._dbContext.SaveChangesAsync();
            }
        }
    }
}
