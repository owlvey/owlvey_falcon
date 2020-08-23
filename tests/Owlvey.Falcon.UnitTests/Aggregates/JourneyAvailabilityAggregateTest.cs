using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;
using static Owlvey.Falcon.UnitTests.TestDataFactory;

namespace Owlvey.Falcon.UnitTests.Aggregates
{
    public class JourneyAvailabilityAggregateTest
    {
        [Fact]
        public void AvailabilityAggregateSuccess() {

            var sourceEntity = new SourceEntity() { };
            sourceEntity.AddSourceItem(800, 1000, OwlveyCalendar.January201903, DateTime.Now, "test",
                 SourceGroupEnum.Availability);


            var entity = new JourneyEntity()
            {
                FeatureMap = new List<JourneyMapEntity>() {
                     new JourneyMapEntity(){
                          Feature = new Core.Entities.FeatureEntity()
                            {
                                Id = 1,
                                Name = "test",
                                Indicators = new List<IndicatorEntity>() { new IndicatorEntity() {
                                        Id  = 1,
                                        Source = sourceEntity
                                    } }
                            }
                     }
                 }
            };
            
            var result = entity.Measure();
            Assert.Equal(0.8m, result.Availability);            
        }
    }
}
