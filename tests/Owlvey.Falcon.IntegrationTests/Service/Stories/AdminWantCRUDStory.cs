using Owlvey.Falcon.IntegrationTests.Service.Scenarios;
using Owlvey.Falcon.IntegrationTests.Setup;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Text;
using TestStack.BDDfy;
using Xunit;

namespace Owlvey.Falcon.IntegrationTests.Service.Stories
{
    [Collection("API Test Collection")]
    [Story(AsA = "Admin",
        IWant = "to be able to mantain the Services information",
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
        public void admin_can_create_service()
        {
            this._scenario = this._container.GetInstance<AdminCanCreateServiceScenario>();
            this._scenario.BDDfy();
        }

        [Fact]
        public void admin_cannot_create_a_service_with_an_existing_name()
        {
            this._scenario = this._container.GetInstance<AdminCannotCreateServiceWithExistingNameScenario>();
            this._scenario.BDDfy();
        }


        [Fact]
        public void admin_can_delete_service()
        {
            this._scenario = this._container.GetInstance<AdminCanDeleteServiceScenario>();
            this._scenario.BDDfy();
        }

        [Fact]
        public void admin_can_update_service()
        {
            this._scenario = this._container.GetInstance<AdminCanUpdateServiceScenario>();
            this._scenario.BDDfy();
        }

        public void Dispose()
        {
            this._container.Dispose();
            if (this._scenario != null) this._scenario.Dispose();
        }
    }
}
