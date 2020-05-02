using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using Xunit;

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
                SourceItems = new List<SourceItemEntity>() {
                     new SourceItemEntity(){ Total = 1000, Good = 800 }
                 }
            };            
            var proportion = entity.MeasureProportion();

            Assert.Equal(0.8m, proportion.Proportion);
        }

        [Fact]
        public void MeasureProportionAvailability()
        {
            var entity = new InteractionSourceEntity()
            {
                Kind = SourceKindEnum.Proportion,
                SourceItems = new List<SourceItemEntity>() {
                     new SourceItemEntity(){ Total = 1000, Good = 800 }
                }
            };
            
            var a = entity.MeasureProportion();
            Assert.Equal(0.8m, a.Proportion);
        }
    }
}
