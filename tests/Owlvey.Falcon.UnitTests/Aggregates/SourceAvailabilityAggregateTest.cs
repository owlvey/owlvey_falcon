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

            var entity = new InteractionSourceEntity()
            {
                Kind = SourceKindEnum.Interaction,            
            };

            entity.SourceItems.Add(InteractionSourceEntity.Factory.CreateInteraction(entity, DateTime.Now, 800, 1000, DateTime.Now, "test"));

            var proportion = entity.MeasureProportion();

            Assert.Equal(0.8m, proportion.Proportion);
        }

        [Fact]
        public void MeasureProportionAvailability()
        {

            var sourceEntity = new InteractionSourceEntity() { };
            sourceEntity.AddSourceItem(800, 1000, OwlveyCalendar.January201903, DateTime.Now, "test");
            
            var a = sourceEntity.MeasureProportion();
            Assert.Equal(0.8m, a.Proportion);
        }
    }
}
