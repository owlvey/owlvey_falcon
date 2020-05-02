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
            int total;
            int good;
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

            total = data.Sum(c => c.Total);
            good = data.Sum(c => c.Good);            
            return new ProportionMeasureValue(total, good);
        }
    }
}
