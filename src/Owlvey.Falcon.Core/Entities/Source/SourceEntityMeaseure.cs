﻿using Owlvey.Falcon.Core.Aggregates;
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
                data = this.SourceItems.Where(c => c.Target >= period.Start && c.Target <= period.End);
            }
            else
            {
                data = this.SourceItems;
            }
            if (data.Count() == 0)
            {
                return new MeasureValue(1, false);
            }
            var proportion = QualityUtils.CalculateAverage(data.Select(c => c.Measure));
            return new MeasureValue(proportion);
        }

        private MeasureValue MeasureAvailabilityExperience(DatePeriodValue period = null)
        {
            IEnumerable<SourceItemEntity> data;
            if (period != null)
            {
                data = this.SourceItems.Where(c => c.Target >= period.Start && c.Target <= period.End);
            }
            else
            {
                data = this.SourceItems;
            }
            if (data.Count() == 0)
            {
                if (this.Kind == SourceKindEnum.Interaction)
                {
                    return new InteractionMeasureValue(1, 0, 0, false);
                }
                else {
                    return new MeasureValue(1, false);
                }
                
            }

            if (this.Kind == SourceKindEnum.Interaction)
            {
                var temp = data.OfType<SourceItemEntity>().ToList();
                var good = temp.Select(c => c.Good.GetValueOrDefault()).Sum();
                var total = temp.Select(c => c.Total.GetValueOrDefault()).Sum();
                var proportion = QualityUtils.CalculateProportion(total, good);
                return new InteractionMeasureValue(proportion, total, good);
            }
            else
            {
                var proportion = QualityUtils.CalculateAverage(data.Select(c => c.Measure));
                return new MeasureValue(proportion);
            }
        }

        public MeasureValue Measure(DatePeriodValue period = null)
        {
            if (this.Group == SourceGroupEnum.Latency)
            {
                return this.MeasureLatency(period);
            }
            else {
                return this.MeasureAvailabilityExperience(period);
            }
        }
    }
}