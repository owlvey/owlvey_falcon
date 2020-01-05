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

        public FeatureCommonComponent(FalconDbContext dbContext) 
        {
            this._dbContext = dbContext;            
        }        
        internal async Task<(decimal queality, decimal availability, decimal latency)> GetAvailabilityByFeature(FeatureEntity entity, DateTime start, DateTime end)
        {
            foreach (var indicator in entity.Indicators)
            {
                var sourceItems = await this._dbContext.GetSourceItems(indicator.SourceId, start, end);
                indicator.Source.SourceItems = sourceItems;
            }
            var agg = new FeatureAvailabilityAggregate(entity);
            var (quality, _, _, availability, latency) = agg.MeasureQuality();
            return (quality, availability, latency);
        }
    }
}
