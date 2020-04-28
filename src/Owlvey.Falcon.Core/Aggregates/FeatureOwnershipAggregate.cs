using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class FeatureOwnershipAggregate
    {
        private readonly IEnumerable<SquadEntity> Squads;
        private readonly IEnumerable<FeatureEntity> Features;
        public FeatureOwnershipAggregate(IEnumerable<SquadEntity> squads, IEnumerable<FeatureEntity> features) {
            this.Squads = squads;
            this.Features = features;
        }
        public (decimal ownership, int total, int assigned) Measure() {            
            if (this.Features.Count() == 0)
            {
                return (1, 0, 0);
            }
            var total = this.Features.Count();
            var state = this.Features.ToDictionary(c => c.Id.Value, c => c.Name);
            foreach (var item in this.Squads.SelectMany(c => c.FeatureMaps))
            {
                if (state.ContainsKey(item.FeatureId))
                {
                    state.Remove(item.FeatureId);
                }
                if (state.Count == 0)
                {
                    break;
                }
            }
            var assigned = total - state.Count();
            return (QualityUtils.CalculateProportion(total, assigned), total, assigned);
        }
    }
}
