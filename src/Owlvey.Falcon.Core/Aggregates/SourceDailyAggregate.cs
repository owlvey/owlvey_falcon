﻿using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;
using System.Linq;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class SourceDailyAggregate
    {
        private readonly SourceEntity Source;
        private readonly DatePeriodValue Period;

        public SourceDailyAggregate(SourceEntity source, DatePeriodValue period)
        {            
            this.Source = source;
            this.Period = period;
        }
                

        public IEnumerable<DayMeasureValue> MeasureAvailability()
        {            
            var result = new List<DayMeasureValue>();
            foreach (var item in this.Period.GetDatesPeriods())
            {                
                var measure = this.Source.Measure(item);
                if (measure.HasData) {
                    result.Add(new DayMeasureValue(item.Start, 
                        new QualityMeasureValue(measure.Availability,
                        measure.Latency, measure.Experience)));
                }    
            }
            return result;
        }
    }
}
