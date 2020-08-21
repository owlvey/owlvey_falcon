using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class FeatureEntity 
    {
        public DebtMeasureValue MeasureDebt(DatePeriodValue period = null) {
            var measure = this.Measure(period);
            var result = new DebtMeasureValue();
            result.AddRange(this.ServiceMaps.Select(c => measure.MeasureDebt(c.Service.GetSLO())).ToList());
            return result;            
        }
        public QualityMeasureValue Measure(DatePeriodValue period = null)
        {            
            var temp  = new List<QualityMeasureValue>();
            
            bool hasData = false;
            foreach (var indicator in this.Indicators)
            {
                QualityMeasureValue quality = indicator.Source.Measure(period);
                
                if (quality.HasData)
                {
                    temp.Add(quality);
                    hasData = true;
                }                
            }
            if (hasData)
            {
                return QualityMeasureValue.Merge(temp);
            }
            else
            {
                return new QualityMeasureValue(false);
            }
        }
    }
}
