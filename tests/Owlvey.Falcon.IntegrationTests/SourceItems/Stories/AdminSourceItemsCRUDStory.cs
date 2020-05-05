using Owlvey.Falcon.IntegrationTests.Setup;
using Owlvey.Falcon.IntegrationTests.SourceItems.Scenarios;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Text;
using TestStack.BDDfy;
using Xunit;

namespace Owlvey.Falcon.IntegrationTests.SourceItems.Stories
{
    [Collection("API Test Collection")]
    [Story(AsA = "Admin",
        IWant = "to be able to mantain the Squads information",
        SoThat = "I can keep updated")]
    public class AdminSourceItemsCRUDStory : IntegrationTestBase, IDisposable, IBaseTest
    {
        private IDisposable _scenario;
        private IContainer _container;

        public AdminSourceItemsCRUDStory()
        {
            this._container = Shell.Instance.Container.CreateChildContainer();
        }

        [Fact]
        public void Admin_can_create_source_item_interaction()
        {
            this._scenario = this._container.GetInstance<AdminCanCreateSourceItemInteractionScenario>();
            this._scenario.BDDfy();
        }
        [Fact]
        public void Admin_can_create_source_item_proportion() {
            this._scenario = this._container.GetInstance<AdminCanCreateSourceItemProportionScenario>();
            this._scenario.BDDfy();
        }
        public void Dispose()
        {
            this._container.Dispose();
            if (this._scenario != null) this._scenario.Dispose();
        }
    }
}
