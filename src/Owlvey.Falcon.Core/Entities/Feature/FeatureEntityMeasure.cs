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
            QualityMeasureValue measure = this.Measure(period);
            var result = new DebtMeasureValue();
            var measures = this.JourneyMaps.Select(c => measure.MeasureDebt(c.Journey.GetSLO())).ToList();
            result.AddRange(measures);
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
