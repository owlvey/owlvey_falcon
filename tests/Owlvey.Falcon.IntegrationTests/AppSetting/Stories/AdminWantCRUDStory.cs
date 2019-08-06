using Owlvey.Falcon.IntegrationTests.AppSetting.Scenarios;
using Owlvey.Falcon.IntegrationTests.Setup;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Text;
using TestStack.BDDfy;
using Xunit;

namespace Owlvey.Falcon.IntegrationTests.AppSetting.Stories
{
    [Collection("API Test Collection")]
    [Story(AsA = "Admin",
        IWant = "to be able to mantain the appsettings information",
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
        public void admin_can_create_setting()
        {
            this._scenario = this._container.GetInstance<AdminCanCreateSettingScenario>();
            this._scenario.BDDfy();
        }

        [Fact]
        public void admin_can_delete_setting()
        {
            this._scenario = this._container.GetInstance<AdminCanDeleteSettingScenario>();
            this._scenario.BDDfy();
        }

        [Fact]
        public void admin_can_delete_an_not_exists_setting()
        {
            this._scenario = this._container.GetInstance<AdminCanDeleteANotExistSettingScenario>();
            this._scenario.BDDfy();
        }

        [Fact]
        public void admin_can_update_setting()
        {
            this._scenario = this._container.GetInstance<AdminCanUpdateSettingScenario>();
            this._scenario.BDDfy();
        }

        [Fact]
        public void admin_can_update_an_not_exists_setting()
        {
            this._scenario = this._container.GetInstance<AdminCanUpdateANotExistSettingScenario>();
            this._scenario.BDDfy();
        }

        [Fact]
        public void admin_can_create_setting_with_existing_key()
        {
            this._scenario = this._container.GetInstance<AdminCanCreateSettingWithExistingKeyScenario>();
            this._scenario.BDDfy();
        }

        public void Dispose()
        {
            this._container.Dispose();
            if (this._scenario != null) this._scenario.Dispose();
        }
    }
}
