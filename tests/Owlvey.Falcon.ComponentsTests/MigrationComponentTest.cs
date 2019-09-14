using Owlvey.Falcon.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
        [Fact]
        public async Task ExporImportFile()
        {

            var container = ComponentTestFactory.BuildContainer();
            var customerComponet = container.GetInstance<CustomerComponent>();
            var migrationComponent = container.GetInstance<MigrationComponent>();

            var result = await customerComponet.CreateCustomer(new Models.CustomerPostRp()
            {
                Name = "test"
            });

            var bytes = File.ReadAllBytes("/Users/Gregory/Downloads/SBP-metadata.xlsx");
            MemoryStream stream = new MemoryStream(bytes);
            stream.Position = 0;

            await migrationComponent.ImportMetadata(result.Id, stream);

            stream = new MemoryStream(bytes);
            stream.Position = 0;

            await migrationComponent.ImportMetadata(result.Id, stream);
        }
    }
}