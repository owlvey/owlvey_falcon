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
            var agg = new SourceAvailabilityAggregate( new SourceEntity() {
                 Kind = SourceKindEnum.Interaction,
                 SourceItems = new List<SourceItemEntity>() {
                     new SourceItemEntity(){ Total = 1000, Good = 800 }
                 }
            });
            var proportion = agg.MeasureAvailability();

            Assert.Equal(0.8m, proportion.Quality);
        }

        [Fact]
        public void MeasureProportionAvailability()
        {
            var agg = new SourceAvailabilityAggregate(new SourceEntity()
            {
                Kind = SourceKindEnum.Proportion,
                SourceItems = new List<SourceItemEntity>() {
                     new SourceItemEntity(){ Total = 1000, Good = 800 }
                }
            });
            var a = agg.MeasureAvailability();
            Assert.Equal(0.8m, a.Quality);
        }
    }
}
