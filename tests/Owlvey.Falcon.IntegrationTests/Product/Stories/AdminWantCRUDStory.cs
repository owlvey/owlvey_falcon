using Owlvey.Falcon.IntegrationTests.Product.Scenarios;
using Owlvey.Falcon.IntegrationTests.Setup;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Text;
using TestStack.BDDfy;
using Xunit;

namespace Owlvey.Falcon.IntegrationTests.Product.Stories
{
    [Collection("API Test Collection")]
    [Story(AsA = "Admin",
        IWant = "to be able to mantain the Products information",
        SoThat = "I can keep updated")]
    public class AdminWantCRUDStory : IntegrationTestBase, IDisposable, IBaseTest
    {
        private IDisposable _scenario;
        private IContainer _container;

        public AdminWantCRUDStory()
        {
            this._container = Shell.Instance.Container.CreateChildContainer();
        }

        [Fact]
        public void admin_can_create_product()
        {
            this._scenario = this._container.GetInstance<AdminCanCreateProductScenario>();
            this._scenario.BDDfy();
        }

        [Fact]
        public void admin_cannot_create_a_product_with_an_existing_name()
        {
            this._scenario = this._container.GetInstance<AdminCannotCreateProductWithExistingNameScenario>();
            this._scenario.BDDfy();
        }

        [Fact]
        public void admin_can_delete_product()
        {
            this._scenario = this._container.GetInstance<AdminCanDeleteProductScenario>();
            this._scenario.BDDfy();
        }

        [Fact]
        public void admin_can_update_product()
        {
            this._scenario = this._container.GetInstance<AdminCanUpdateProductScenario>();
            this._scenario.BDDfy();
        }

        public void Dispose()
        {
            this._container.Dispose();
            if (this._scenario != null) this._scenario.Dispose();
        }
    }
}
