using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class SourceMetricsAggregate
    {
        private readonly IEnumerable<SourceEntity> Sources;
        public SourceMetricsAggregate(IEnumerable<SourceEntity> sources) {
            this.Sources = sources;
        }
        public (decimal availability, int availabilityTotal, int availabilityGood, decimal availabilityInteractions, decimal latency, decimal experience) Execute() {
            var availabilityMeasures = new List<MeasureValue>();
            int availabilityInteractionTotal = 0;
            int availabilityInteractionGood = 0;
            foreach (var source in this.Sources.Where(c=>c.Group == SourceGroupEnum.Availability))
            {
                //TODO trabajar outliers
                var measure = source.Measure();
                if (measure.HasData && measure.Value > 0) {
                    availabilityMeasures.Add(measure);                    
                    if (source.Kind == SourceKindEnum.Interaction) {
                        availabilityInteractionTotal += (source.SourceItems.Sum(c => c.Total.GetValueOrDefault()));
                        availabilityInteractionGood += (source.SourceItems.Sum(c => c.Good.GetValueOrDefault()));
                    }
                }                
            }
            var availabilityInteraction = QualityUtils.CalculateProportion(availabilityInteractionTotal, availabilityInteractionGood);
            var availability = QualityUtils.CalculateAverage(availabilityMeasures.Select(c=>c.Value));

            var latencyMeasures = new List<MeasureValue>();
            foreach (var source in this.Sources.Where(c => c.Group == SourceGroupEnum.Latency))
            {
                //TODO trabajar outliers
                var measure = source.Measure();
                if (measure.HasData && measure.Value > 0)
                {
                    latencyMeasures.Add(measure);
                }
            }
            var latency = QualityUtils.CalculateAverage(latencyMeasures.Select(c => c.Value));

            var experienceMeasures = new List<MeasureValue>();
            foreach (var source in this.Sources.Where(c => c.Group == SourceGroupEnum.Latency))
            {
                //TODO trabajar outliers
                var measure = source.Measure();
                if (measure.HasData && measure.Value > 0)
                {
                    latencyMeasures.Add(measure);
                }
            }
            var experience = QualityUtils.CalculateAverage(experienceMeasures.Select(c => c.Value));

            return (availability, availabilityInteractionTotal, availabilityInteractionGood,
                availabilityInteraction, latency, experience);
        }

    }
}
