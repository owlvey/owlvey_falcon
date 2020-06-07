using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ProductEntity 
    {
        public DebtMeasureValue Measure(DatePeriodValue period = null) {
            var measure = new DebtMeasureValue();
            foreach (var service in this.Services)
            {
                measure.Add(service.Measure(period));
            }
            return measure;
        }
        public (decimal coverage, int total, int assigned) MeasureCoverage()
        {
            var features = this.Features;
            if (features.Count == 0)
            {
                return (1, 0, 0);
            }
            var total = features.Count;
            var state = features.ToDictionary(c => c.Id.Value, c => c.Name);
            foreach (var item in this.Services.SelectMany(c => c.FeatureMap))
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
            var coverage = total - state.Count();
            return (QualityUtils.CalculateProportion(total, coverage), total, coverage);
        }

        public (decimal utilization, int total, int assigned) MeasureUtilization() {
            var sources = this.Sources;
            if (sources.Count == 0)
            {
                return (1, 0, 0);
            }
            var total = sources.Count;
            var state = sources.ToDictionary(c => c.Id.Value, c => c.Name);
            foreach (var item in this.Features.SelectMany(c => c.Indicators))
            {
                if (state.ContainsKey(item.SourceId))
                {
                    state.Remove(item.SourceId);
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
