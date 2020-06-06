using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Models.Series;
using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class ScalabilityMeasureAggregate
    {
        public IEnumerable<SourceItemEntity> Items;
        public ScalabilityMeasureAggregate(IEnumerable<SourceItemEntity> items, DatePeriodValue period = null) {
            if (period != null)
            {
                this.Items = items.Where(c=>c.Target>= period.Start && c.Target<= period.End);
            }
            else {
                this.Items = items;
            }
            
        }

        public (double correlation, 
            int[] dailyTotal, 
            int[] dailyBad, double b0, double b1, double r2,
            List<DateInteractionModel> dailyInteractions            
            ) Measure() {            
            var groups = this.Items.GroupBy(c => c.Target).OrderBy(c => c.Key).ToList();
            var totals = groups.Select(c => c.Sum(d => d.Total.GetValueOrDefault())).ToArray();
            var bad = groups.Select(c => c.Sum(d => d.Bad.GetValueOrDefault())).ToArray();
            var dailyInteractions = new List<DateInteractionModel>();
            foreach (var item in groups)
            {
                dailyInteractions.Add(
                    new DateInteractionModel(item.Key,
                     item.Sum(c => c.Total).GetValueOrDefault(),
                     item.Sum(c => c.Good).GetValueOrDefault())
                );
            }

            var pairs = totals.Zip(bad).OrderBy(c => c.First);
            totals = pairs.Select(c => c.First).ToArray();
            bad = pairs.Select(c => c.Second).ToArray();

            double correlation = QualityUtils.MeasureCorrelation(totals, bad);
            var (b0, b1, r2) = QualityUtils.LinealRegression(totals, bad);
            return (correlation, totals, bad, b0, b1, r2, dailyInteractions);
        }
    }
}
