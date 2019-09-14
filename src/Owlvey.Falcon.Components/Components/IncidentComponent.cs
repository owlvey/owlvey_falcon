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

        public async Task<IncidentGetListRp> Post(int productId, string title) {
            var createdBy = this._identityService.GetIdentity();
            var product = await this._dbContext.Products.Where(c => c.Id == productId).SingleAsync();
            var entity = IncidentEntity.Factory.Create(title, this._datetimeGateway.GetCurrentDateTime(), createdBy, product);
            this._dbContext.Incidents.Add(entity);
            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<IncidentGetListRp>(entity);
        }
    }
}
