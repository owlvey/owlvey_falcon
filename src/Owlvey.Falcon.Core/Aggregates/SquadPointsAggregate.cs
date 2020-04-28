using System;
using Owlvey.Falcon.Core.Entities;
using System.Linq;
using System.Collections.Generic;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class SquadPointsAggregate
    {
        private SquadEntity _squad;
        public SquadPointsAggregate(SquadEntity squad)
        {
            this._squad = squad;
        }

        public IEnumerable<(ProductEntity product, ServiceEntity service, FeatureEntity feature,
            decimal quality,
            decimal points)> MeasurePoints() {

            var result = new List<(ProductEntity product, ServiceEntity service, FeatureEntity feature, decimal availability, decimal points)>();

            foreach (var featureMap in this._squad.FeatureMaps)
            {   
                var quality = featureMap.Feature.MeasureQuality().Quality;

                foreach (var serviceMap in featureMap.Feature.ServiceMaps)
                {
                    var service = serviceMap.Service;
                    var impact = QualityUtils.MeasureImpact(service.Slo);
                    var points = QualityUtils.MeasurePoints(service.Slo, quality);

                    result.Add( (service.Product, service, featureMap.Feature, quality, points) );
                }
            }
            return result;
        }
    }
}
