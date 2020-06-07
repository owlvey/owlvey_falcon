﻿using System;
using Owlvey.Falcon.Core.Entities;
using System.Linq;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class SquadPointsAggregate
    {
        private SquadEntity _squad;
        public SquadPointsAggregate(SquadEntity squad)
        {
            this._squad = squad;
        }

        public IEnumerable<
            (
                ProductEntity product, ServiceEntity service, FeatureEntity feature,
                QualityMeasureValue quality,
                DebtMeasureValue debt)> Measure() {

            var result = new List<(ProductEntity product, 
                ServiceEntity service, 
                FeatureEntity feature,
                QualityMeasureValue quality,
                DebtMeasureValue debt)>();

            foreach (var featureMap in this._squad.FeatureMaps)
            {   
                var quality = featureMap.Feature.Measure();
                foreach (var serviceMap in featureMap.Feature.ServiceMaps)
                {                    
                    var service = serviceMap.Service;
                    var debt = quality.MeasureDebt(service.GetSLO());                    
                    result.Add((service.Product, service, featureMap.Feature, quality, debt));
                }
            }
            return result;
        }
    }
}
