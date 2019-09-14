using Owlvey.Falcon.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class MigrationComponentTest
    {
        [Fact]
        public async Task ExporImport()
        {            
            
            var container = ComponentTestFactory.BuildContainer();
            var customerComponet = container.GetInstance<CustomerComponent>();
            var migrationComponent = container.GetInstance<MigrationComponent>();

            var result = await customerComponet.CreateCustomer(new Models.CustomerPostRp()
            {
                Name = "test"
            });

            var (customer, stream) = await migrationComponent.ExportMetadataExcel(result.Id);

            stream.Position = 0;
            await migrationComponent.ImportMetadata(customer.Id.Value, stream);

        }
 
    }
}