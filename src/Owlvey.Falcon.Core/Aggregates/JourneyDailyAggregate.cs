using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;
using System.Linq;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class JourneyDailyAggregate
    {
        public JourneyEntity Journey { get; protected set; }
                
        private readonly DatePeriodValue Period;
        public JourneyDailyAggregate(JourneyEntity entity, DatePeriodValue period)
        {
            this.Journey = entity;
            this.Period = period;
        }

        public (
            IEnumerable<DayMeasureValue> journeyDaily,
            IEnumerable<(FeatureEntity, IEnumerable<DayMeasureValue>)> featuresDaily) MeasureQuality
        {
            get
            {

                List<DayMeasureValue> journeyResult = new List<DayMeasureValue>();

                foreach (var period in this.Period.GetDatesPeriods())
                {
                    var measure = this.Journey.Measure(period);
                    if (measure.HasData)
                    {
                        journeyResult.Add(new DayMeasureValue(period.Start, measure));
                    }
                }

                var featuresResult = new List<(FeatureEntity, IEnumerable<DayMeasureValue>)>();

                foreach (var map in this.Journey.FeatureMap)
                {
                    List<DayMeasureValue> temp = new List<DayMeasureValue>();
                    foreach (var item in this.Period.GetDatesPeriods())
                    {
                        var measure = map.Feature.Measure(item);
                        if (measure.HasData)
                        {
                            temp.Add(new DayMeasureValue(item.Start, measure));
                        }
                    }
                    featuresResult.Add((map.Feature, temp));
                }
                return (journeyResult, featuresResult);
            }
        }
    }
}
