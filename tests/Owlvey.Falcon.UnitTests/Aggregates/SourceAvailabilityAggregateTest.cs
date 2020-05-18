using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;
using static Owlvey.Falcon.UnitTests.TestDataFactory;

namespace Owlvey.Falcon.UnitTests.Aggregates
{
    public class SourceAvailabilityAggregateTest
    {
        [Fact]
        public void MeasureFeatureAvailability()
        {

            var entity = new SourceEntity()
            {
                Kind = SourceKindEnum.Interaction,            
            };

            entity.SourceItems.Add(SourceEntity.Factory.CreateInteraction(entity, DateTime.Now, 800, 1000, DateTime.Now, "test"));

            var proportion = entity.Measure();

            Assert.Equal(0.8m, proportion.Value);
        }

        [Fact]
        public void MeasureProportionAvailability()
        {

            var sourceEntity = new SourceEntity() { };
            sourceEntity.AddSourceItem(800, 1000, OwlveyCalendar.January201903, DateTime.Now, "test");
            
            var a = sourceEntity.Measure();
            Assert.Equal(0.8m, a.Value);
        }
    }
}
