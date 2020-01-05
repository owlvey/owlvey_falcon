using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;
using static Owlvey.Falcon.UnitTests.TestDataFactory;

namespace Owlvey.Falcon.UnitTests.Aggregates
{    
    public class FeatureAvailabilityAggregateTest
    {
        [Fact]
        public void FeatureDateAvailabilityAggregateSuccess() {

            var agg = new FeatureAvailabilityAggregate(new FeatureEntity() {
                  Id = 1,
                  Name = "test",
                  Indicators = new List<IndicatorEntity>() { new IndicatorEntity() {
                        Id  = 1,
                        Source = new SourceEntity(){
                             SourceItems = new List<SourceItemEntity>(){
                                  new SourceItemEntity(){
                                       Good = 800, Total = 1000,
                                       Start = OwlveyCalendar.January201903,
                                       End = OwlveyCalendar.January201905
                                  }
                             }
                        }
                  } }
            });
            var result = agg.MeasureAvailability();

            Assert.Equal(1000, result.total);
            Assert.Equal(800, result.good);
            Assert.Equal(0.8m, result.availability);
            
        }


        [Fact]
        public void FeatureAvailabilityMix()
        {

            var agg = new FeatureAvailabilityAggregate(new FeatureEntity()
            {
                Id = 1,
                Name = "test",
                Indicators = new List<IndicatorEntity>() {
                    new IndicatorEntity() {
                        Id  = 1,
                        Source = new SourceEntity(){
                             Kind = SourceKindEnum.Interaction,
                             SourceItems = new List<SourceItemEntity>(){
                                  new SourceItemEntity(){
                                       Good = 800, Total = 1000,
                                       Start = OwlveyCalendar.January201903,
                                       End = OwlveyCalendar.January201905
                                  }
                             }
                        }
                    },
                    new IndicatorEntity() {
                        Id  = 2,
                        Source = new SourceEntity(){
                             Kind = SourceKindEnum.Proportion,
                             SourceItems = new List<SourceItemEntity>(){
                                  new SourceItemEntity(){
                                       Good = 90, Total = 100,
                                       Start = OwlveyCalendar.January201903,
                                       End = OwlveyCalendar.January201905
                                  }
                             }
                        }
                    }
                }
            });
            var result = agg.MeasureAvailability();

            Assert.Equal(1000, result.total);
            Assert.Equal(800, result.good);
            Assert.Equal(0.72m, result.availability);

        }
    }
}
