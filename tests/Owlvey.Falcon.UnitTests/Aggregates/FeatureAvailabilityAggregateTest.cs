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

            var sourceEntity = new SourceEntity(){};
            sourceEntity.AddSourceItem(800, 1000, OwlveyCalendar.January201903, DateTime.Now, "test");

            var entity = new FeatureEntity()
            {
                Id = 1,
                Name = "test",
                Indicators = new List<IndicatorEntity>() { new IndicatorEntity() {
                        Id  = 1,
                        Source = sourceEntity
                  } }
            };
            
            var result = entity.Measure();
            
            Assert.Equal(0.8m, result.Availability);
            
        }


        [Fact]
        public void FeatureAvailabilityMix()
        {
            var Source_A = new SourceEntity()
            {
                Kind = SourceKindEnum.Interaction                
            };
            Source_A.AddSourceItem(800, 1000, OwlveyCalendar.January201903, DateTime.Now, "test");

            var Source_B = new SourceEntity()
            {
                Kind = SourceKindEnum.Interaction            
            };
            Source_B.AddSourceItem(90, 100, OwlveyCalendar.January201903, DateTime.Now, "test");
            var Indicators = new List<IndicatorEntity>() {
                    new IndicatorEntity() {
                        Id  = 1,
                        Source = Source_A
                    },
                    new IndicatorEntity() {
                        Id  = 2,
                        Source = Source_B                        
                    }
                };
            var entity = new FeatureEntity()
            {
                Id = 1,
                Name = "test",
                Indicators = Indicators                
            };

            var result = entity.Measure();
            Assert.Equal(0.8m, result.Availability);

        }
    }
}
