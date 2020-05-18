using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class FeatureEntity 
    {
        public QualityMeasureValue Measure(DatePeriodValue period = null)
        {
            var result = new List<decimal>();
            var availability = new List<decimal>();
            var latency = new List<decimal>();
            var experience = new List<decimal>();
            foreach (var indicator in this.Indicators)
            {                
                MeasureValue measure;
                if (period != null)
                {
                    measure = indicator.Source.Measure(period);
                }
                else
                {
                    measure = indicator.Source.Measure();
                }

                if (measure.HasData)
                {
                    result.Add(measure.Value);
                    if (indicator.Source.Group == SourceGroupEnum.Availability)
                    {
                        availability.Add(measure.Value);
                    }
                    else if (indicator.Source.Group == SourceGroupEnum.Latency)
                    {
                        latency.Add(measure.Value);
                    }
                    else if (indicator.Source.Group == SourceGroupEnum.Experience) {
                        experience.Add(measure.Value);
                    }
                }
            }
            if (result.Count > 0)
            {
                return new QualityMeasureValue(                    
                    QualityUtils.CalculateMinimum(availability, 3),
                    QualityUtils.CalculateMaximum(latency, 3),
                    QualityUtils.CalculateMinimum(experience, 3), true);
            }
            else
            {
                return new QualityMeasureValue(false);
            }
        }
    }
}
