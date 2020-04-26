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
                                       Target = OwlveyCalendar.January201903                                       
                                  }
                             }
                        }
                  } }
            });
            var result = agg.MeasureQuality();
            
            Assert.Equal(0.8m, result.Quality);
            
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
                                       Target = OwlveyCalendar.January201903                                       
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
                                       Target = OwlveyCalendar.January201903                                       
                                  }
                             }
                        }
                    }
                }
            });
            var result = agg.MeasureQuality();
            Assert.Equal(0.8m, result.Quality);

        }
    }
}
