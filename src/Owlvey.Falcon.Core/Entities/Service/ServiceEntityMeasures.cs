using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ServiceEntity
    {        
        public ServiceQualityMeasureValue Measure(DatePeriodValue period = null)
        {            
            var resultAvailability = new List<decimal>();
            var resultLatency = new List<decimal>();
            var resultExperience = new List<decimal>(); 

            foreach (var map in this.FeatureMap)
            {                
                QualityMeasureValue measure;
                if (period != null)
                {
                    measure = map.Feature.Measure(period);
                }
                else
                {
                    measure = map.Feature.Measure();
                }
                if (measure.HasData)
                {                    
                    resultAvailability.Add(measure.Availability);
                    resultLatency.Add(measure.Latency);
                    resultExperience.Add(measure.Experience); 
                }
            }

            if (resultAvailability.Count > 0 || resultExperience.Count>0 || resultLatency.Count >0)
            {
                return new ServiceQualityMeasureValue(                    
                    this.AvailabilitySlo,
                    this.LatencySlo, 
                    this.ExperienceSlo,
                    QualityUtils.CalculateMinimum(resultAvailability, round: 3),
                    QualityUtils.CalculateMinimum(resultLatency, round: 3),
                    QualityUtils.CalculateMinimum(resultExperience, round: 3));
            }
            else
            {
                return new ServiceQualityMeasureValue(this.AvailabilitySlo,
                    this.LatencySlo, this.ExperienceSlo, false);
            }
        }
    }
}
