using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;
using static Owlvey.Falcon.UnitTests.TestDataFactory;

namespace Owlvey.Falcon.UnitTests.Aggregates
{
    public class ServiceAvailabilityAggregateTest
    {
        [Fact]
        public void AvailabilityAggregateSuccess() {


            var entity = new ServiceEntity()
            {
                FeatureMap = new List<ServiceMapEntity>() {
                     new ServiceMapEntity(){
                          Feature = new Core.Entities.FeatureEntity()
                            {
                                Id = 1,
                                Name = "test",
                                Indicators = new List<IndicatorEntity>() { new IndicatorEntity() {
                                        Id  = 1,
                                        Source = new InteractionSourceEntity(){
                                                SourceItems = new List<SourceItemEntity>(){
                                                    new InteractionSourceItemEntity(){
                                                        Good = 800, Total = 1000,
                                                        Target = OwlveyCalendar.January201903
                                                    }
                                                }
                                        }
                                    } }
                            }
                     }
                 }
            };
            
            var result = entity.MeasureQuality();
            Assert.Equal(0.8m, result.Quality);            
        }
    }
}
