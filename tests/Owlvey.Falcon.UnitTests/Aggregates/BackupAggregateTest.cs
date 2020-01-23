using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Aggregates
{
    public class BackupAggregateTest
    {
        [Fact]
        public void BackupOrganization()
        {
            var customer = TestDataFactory.BuildCustomer(false);

            var aggregate = new BackupAggregate( 
                new List<UserEntity>(), 
                new List<CustomerEntity>() { customer },
                new List<ServiceEntity>(), 
                new List<FeatureEntity>(),
                new List<SourceEntity>(),
                new List<SourceItemEntity>());

            var result = aggregate.Execute();

            Assert.NotEmpty(result.Organizations); 

        }
    }
}
