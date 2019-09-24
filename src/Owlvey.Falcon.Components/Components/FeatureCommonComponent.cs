using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Aggregates;

namespace Owlvey.Falcon.Components
{
    internal class FeatureCommonComponent 
    {
        private readonly FalconDbContext _dbContext;
        private readonly IDateTimeGateway _dateTimeGateway;

        public FeatureCommonComponent(FalconDbContext dbContext, 
            IDateTimeGateway dateTimeGateway) 
        {
            this._dbContext = dbContext;
            this._dateTimeGateway = dateTimeGateway;
        }        
        internal async Task<decimal> GetAvailabilityByFeature(FeatureEntity entity, DateTime start, DateTime end)
        {
            foreach (var indicator in entity.Indicators)
            {
                var sourceItems = this._dbContext.GetSourceItems(indicator.SourceId, start, end);
                indicator.Source.SourceItems = sourceItems;
            }
            var agg = new FeatureDateAvailabilityAggregate(entity);

            return agg.MeasureAvailability();

        }
    }
}
