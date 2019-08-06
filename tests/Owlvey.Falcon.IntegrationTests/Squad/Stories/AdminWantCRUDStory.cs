using Owlvey.Falcon.IntegrationTests.Squad.Scenarios;
using Owlvey.Falcon.IntegrationTests.Setup;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Text;
using TestStack.BDDfy;
using Xunit;

namespace Owlvey.Falcon.IntegrationTests.Squad.Stories
{
    [Collection("API Test Collection")]
    [Story(AsA = "Admin",
        IWant = "to be able to mantain the Squads information",
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
        public void admin_can_create_squad()
        {
            this._scenario = this._container.GetInstance<AdminCanCreateSquadScenario>();
            this._scenario.BDDfy();
        }

        [Fact]
        public void admin_can_add_an_member()
        {
            this._scenario = this._container.GetInstance<AdminCanAddAnMemberToSquadScenario>();
            this._scenario.BDDfy();
        }

        [Fact]
        public void admin_can_remove_an_member()
        {
            this._scenario = this._container.GetInstance<AdminCanRemoveAnMemberToSquadScenario>();
            this._scenario.BDDfy();
        }

        [Fact]
        public void admin_can_delete_squad()
        {
            this._scenario = this._container.GetInstance<AdminCanDeleteSquadScenario>();
            this._scenario.BDDfy();
        }

        [Fact]
        public void admin_can_update_squad()
        {
            this._scenario = this._container.GetInstance<AdminCanUpdateSquadScenario>();
            this._scenario.BDDfy();
        }

        public void Dispose()
        {
            this._container.Dispose();
            if (this._scenario != null) this._scenario.Dispose();
        }
    }
}
