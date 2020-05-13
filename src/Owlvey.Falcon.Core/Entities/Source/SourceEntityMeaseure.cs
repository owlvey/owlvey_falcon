using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SourceEntity
    {
        public ProportionMeasureValue MeasureProportion(DatePeriodValue period = null)
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
                return new ProportionMeasureValue(1, false);
            }

            if (this.Kind == SourceKindEnum.Interaction) {
                var temp = data.OfType<InteractionSourceItemEntity>().ToList();
                var good = temp.Select(c => c.Good).Sum();
                var total = temp.Select(c => c.Total).Sum();
                var proportion = QualityUtils.CalculateProportion(total, good);
                return new ProportionMeasureValue(proportion);
            }
            else {
                var proportion = QualityUtils.CalculateAverage(data.Select(c => c.Proportion));
                return new ProportionMeasureValue(proportion);
            }            
        }
    }
}
