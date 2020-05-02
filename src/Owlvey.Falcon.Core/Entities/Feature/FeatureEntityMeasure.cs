using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class FeatureEntity 
    {
        public QualityMeasureValue MeasureQuality(DatePeriodValue period = null)
        {
            var result = new List<decimal>();
            var availability = new List<decimal>();
            var latency = new List<decimal>();
            foreach (var indicator in this.Indicators)
            {                
                QualityMeasureValue proportion;
                if (period != null)
                {
                    proportion = indicator.Source.MeasureQuality(period);
                }
                else
                {
                    proportion = indicator.Source.MeasureQuality();
                }

                if (proportion.HasData)
                {
                    result.Add(proportion.Quality);
                    if (indicator.Source.Group == SourceGroupEnum.Availability)
                    {
                        availability.Add(proportion.Quality);
                    }
                    else if (indicator.Source.Group == SourceGroupEnum.Latency)
                    {
                        latency.Add(proportion.Quality);
                    }
                }
            }
            if (result.Count > 0)
            {
                return new QualityMeasureValue(                    
                    QualityUtils.CalculateMinimum(availability, 3),
                    QualityUtils.CalculateMinimum(latency, 3), true);
            }
            else
            {
                return new QualityMeasureValue(1, 1, false);
            }
        }
    }
}
