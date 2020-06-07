using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;
using static Owlvey.Falcon.UnitTests.TestDataFactory;

namespace Owlvey.Falcon.UnitTests.Aggregates
{
    public class SquadPointsAggregateTest
    {
        [Fact]
        public void SquadPointsSuccess() {

            var squad = new SquadPointsAggregate(new SquadEntity() {
                 FeatureMaps = new List<SquadFeatureEntity>() {
                     new SquadFeatureEntity(){
                          
                          Feature = new FeatureEntity() {
                                  Id = 1,
                                  Name = "test",
                                  ServiceMaps = new List<ServiceMapEntity>(){
                                      new ServiceMapEntity(){
                                           Service = new ServiceEntity(){
                                                AvailabilitySlo = 0.99m
                                           }
                                      }
                                  },
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
                            }
                     }
                 }
            });

            var result = squad.Measure();

            Assert.NotEmpty(result);

        }
    }
}
