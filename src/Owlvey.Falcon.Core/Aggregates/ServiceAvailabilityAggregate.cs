using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;
using System.Linq;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class ServiceQualityAggregate
    {
        private readonly ServiceEntity Service;                

        public ServiceQualityAggregate(ServiceEntity service)
        {            
            this.Service = service;                        
        }
        public QualityMeasureValue MeasureQuality(DateTime? start = null, DateTime? end = null)
        {
            var result = new List<decimal>();
            var resultAvailability = new List<decimal>();
            var resultLatency = new List<decimal>();             

            foreach (var map in this.Service.FeatureMap)
            {
                var agg = new FeatureAvailabilityAggregate(map.Feature);
                QualityMeasureValue measure;
                if (start.HasValue && end.HasValue)
                {
                    measure = agg.MeasureQuality(start, end);                    
                }
                else {
                    measure = agg.MeasureQuality();
                }
                if (measure.HasData) {
                    result.Add(measure.Quality);
                    resultAvailability.Add(measure.Availability);
                    resultLatency.Add(measure.Latency);
                }                
            }
            
            if (result.Count > 0)
            {    
                return new QualityMeasureValue(
                    QualityUtils.CalculateMinimumAvailability(result, round: 3),
                    QualityUtils.CalculateMinimumAvailability(resultAvailability, round: 3),
                    QualityUtils.CalculateMinimumAvailability(resultLatency, round: 3));
            }
            else {
                return new QualityMeasureValue(1, 1, 1, false);
            }            
        }
    }
}
