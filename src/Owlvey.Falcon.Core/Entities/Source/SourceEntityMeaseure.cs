using MathNet.Numerics.Statistics;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SourceEntity
    {
        private MeasureValue MeasureLatency(DatePeriodValue period = null) {
            IEnumerable<SourceItemEntity> data;
            if (period != null)
            {
                data = this.SourceItems.Where(c => c.Group == SourceGroupEnum.Latency &&
                                                   c.Target >= period.Start && c.Target <= period.End);
            }
            else
            {
                data = this.SourceItems.Where(c=>c.Group == SourceGroupEnum.Latency);
            }
            if (data.Count() == 0)
            {
                return new MeasureValue(101, false);
            }
            var proportion = QualityUtils.CalculateAverage(data.Select(c => c.Measure));
            return new MeasureValue(proportion);
        }

        private MeasureValue MeasureExperience(DatePeriodValue period = null)
        {
            IEnumerable<SourceItemEntity> data;
            if (period != null)
            {
                data = this.SourceItems.Where(c => c.Group == SourceGroupEnum.Experience && c.Target >= period.Start && c.Target <= period.End);
            }
            else
            {
                data = this.SourceItems.Where(c => c.Group == SourceGroupEnum.Experience);
            }
            if (data.Count() == 0)
            {
                return new InteractionMeasureValue(1, 0, 0, false);
            }
            var good = data.Select(c => c.Good.GetValueOrDefault()).Sum();
            var total = data.Select(c => c.Total.GetValueOrDefault()).Sum();
            var measure = QualityUtils.CalculateAverage(data.Select(c => c.Measure));
            return new InteractionMeasureValue(measure, total, good);            
        }
        private InteractionMeasureValue MeasureAvailability(DatePeriodValue period = null)
        {
            IEnumerable<SourceItemEntity> data;
            if (period != null)
            {
                data = this.SourceItems.Where(c => c.Group == SourceGroupEnum.Availability &&  c.Target >= period.Start && c.Target <= period.End);
            }
            else
            {
                data = this.SourceItems.Where(c=> c.Group == SourceGroupEnum.Availability);
            }
            if (data.Count() == 0)
            {                
                 return new InteractionMeasureValue(1, 0, 0, false);
            }
            var good = data.Select(c => c.Good.GetValueOrDefault()).Sum();
            var total = data.Select(c => c.Total.GetValueOrDefault()).Sum();            
            var measure = QualityUtils.CalculateAverage(data.Select(c => c.Measure));
            return new InteractionMeasureValue(measure, total, good);
        }

        public QualityMeasureValue Measure(DatePeriodValue period = null)
        {
            
            return new QualityMeasureValue(
                this.MeasureAvailability(period), 
                this.MeasureLatency(period), 
                this.MeasureExperience(period));
            
        }
        public double? MeasureDailyCorrelation(DatePeriodValue period = null) {
            if (this.SourceItems.Count == 0) {
                return 0;
            }            
            else {
                
                if (period == null ) {
                    period = new DatePeriodValue(
                        this.SourceItems.Min(c => c.Target),
                        this.SourceItems.Max(c => c.Target)
                        );
                }

                var groups = this.SourceItems.Where(c => c.Target >= period.Start && c.Target <= period.End)
                    .GroupBy(c => c.Target.Date).OrderBy(c=>c.Key);

                var totals = groups.Select(c => c.Sum(c => (double)c.Total.GetValueOrDefault())).ToArray();
                var bad = groups.Select(c => c.Sum(c =>  
                        (double)(c.Bad.GetValueOrDefault())
                        )).ToArray();
                var correlation = QualityUtils.MeasureCorrelation(totals, bad);
                return correlation;               
                
            }
        }
    }
}
