using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ServiceEntity
    {        
        public ServiceQualityMeasureValue MeasureQuality(DatePeriodValue period = null)
        {
            var result = new List<decimal>();
            var resultAvailability = new List<decimal>();
            var resultLatency = new List<decimal>();
            var resultExperience = new List<decimal>(); 

            foreach (var map in this.FeatureMap)
            {                
                QualityMeasureValue measure;
                if (period != null)
                {
                    measure = map.Feature.MeasureQuality(period);
                }
                else
                {
                    measure = map.Feature.MeasureQuality();
                }
                if (measure.HasData)
                {
                    result.Add(measure.Quality);
                    resultAvailability.Add(measure.Availability);
                    resultLatency.Add(measure.Latency);
                    resultExperience.Add(measure.Experience); 
                }
            }

            if (result.Count > 0)
            {
                return new ServiceQualityMeasureValue(                    
                    this.Slo,
                    QualityUtils.CalculateMinimum(resultAvailability, round: 3),
                    QualityUtils.CalculateMinimum(resultLatency, round: 3),
                    QualityUtils.CalculateMinimum(resultExperience, round: 3));
            }
            else
            {
                return new ServiceQualityMeasureValue(this.Slo, 1, 1, 1, false);
            }
        }
    }
}
