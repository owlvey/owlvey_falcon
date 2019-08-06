using Owlvey.Falcon.IntegrationTests.User.Scenarios;
using Owlvey.Falcon.IntegrationTests.Setup;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Text;
using TestStack.BDDfy;
using Xunit;

namespace Owlvey.Falcon.IntegrationTests.User.Stories
{
    [Collection("API Test Collection")]
    [Story(AsA = "Admin",
        IWant = "to be able to mantain the Users information",
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
        public void admin_can_create_user()
        {
            this._scenario = this._container.GetInstance<AdminCanCreateUserScenario>();
            this._scenario.BDDfy();
        }

        [Fact]
        public void admin_can_find_user()
        {
            this._scenario = this._container.GetInstance<AdminCanFindUserScenario>();
            this._scenario.BDDfy();
        }

        public void Dispose()
        {
            this._container.Dispose();
            if (this._scenario != null) this._scenario.Dispose();
        }
    }
}
