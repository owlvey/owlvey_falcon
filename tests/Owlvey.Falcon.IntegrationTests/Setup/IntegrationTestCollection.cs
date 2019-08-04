using Owlvey.Falcon.IntegrationTests.Setup;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Owlvey.Falcon.IntegrationTests.Setup
{
    [CollectionDefinition("API Test Collection")]
    public class IntegrationTestCollection : ICollectionFixture<TestSetup>
    {

    }
}
