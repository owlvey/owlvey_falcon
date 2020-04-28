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
            var entity = new SourceEntity()
            {
                Kind = SourceKindEnum.Interaction,
                SourceItems = new List<SourceItemEntity>() {
                     new SourceItemEntity(){ Total = 1000, Good = 800 }
                 }
            };            
            var proportion = entity.MeasureQuality();

            Assert.Equal(0.8m, proportion.Quality);
        }

        [Fact]
        public void MeasureProportionAvailability()
        {
            var entity = new SourceEntity()
            {
                Kind = SourceKindEnum.Proportion,
                SourceItems = new List<SourceItemEntity>() {
                     new SourceItemEntity(){ Total = 1000, Good = 800 }
                }
            };
            
            var a = entity.MeasureQuality();
            Assert.Equal(0.8m, a.Quality);
        }
    }
}
