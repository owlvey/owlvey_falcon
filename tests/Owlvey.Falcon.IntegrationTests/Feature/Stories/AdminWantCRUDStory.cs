using Owlvey.Falcon.IntegrationTests.Feature.Scenarios;
using Owlvey.Falcon.IntegrationTests.Setup;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Text;
using TestStack.BDDfy;
using Xunit;

namespace Owlvey.Falcon.IntegrationTests.Feature.Stories
{
    [Collection("API Test Collection")]
    [Story(AsA = "Admin",
        IWant = "to be able to mantain the Features information",
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
        public void admin_can_create_feature()
        {
            this._scenario = this._container.GetInstance<AdminCanCreateFeatureScenario>();
            this._scenario.BDDfy();
        }

        [Fact]
        public void admin_cannot_create_a_feature_with_an_existing_name()
        {
            this._scenario = this._container.GetInstance<AdminCannotCreateFeatureWithExistingNameScenario>();
            this._scenario.BDDfy();
        }

        [Fact]
        public void admin_can_delete_feature()
        {
            this._scenario = this._container.GetInstance<AdminCanDeleteFeatureScenario>();
            this._scenario.BDDfy();
        }

        [Fact]
        public void admin_can_update_feature()
        {
            this._scenario = this._container.GetInstance<AdminCanUpdateFeatureScenario>();
            this._scenario.BDDfy();
        }

        public void Dispose()
        {
            this._container.Dispose();
            if (this._scenario != null) this._scenario.Dispose();
        }
    }
}
