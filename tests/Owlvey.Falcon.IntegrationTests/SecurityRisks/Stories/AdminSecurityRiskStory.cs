using Owlvey.Falcon.IntegrationTests.SecurityThreats.Scenarios;
using Owlvey.Falcon.IntegrationTests.Setup;
using Owlvey.Falcon.IntegrationTests.SourceItems.Scenarios;
using StructureMap;
using System;
using TestStack.BDDfy;
using Xunit;

namespace Owlvey.Falcon.IntegrationTests.SecurityThreats.Stories
{
    [Collection("API Test Collection")]
    [Story(AsA = "Admin",
        IWant = "to be able to mantain the Security Threat information",
        SoThat = "I can keep updated")]
    public class AdminSecurityRiskStory : IntegrationTestBase, IDisposable, IBaseTest
    {
        private IDisposable _scenario;
        private IContainer _container;

        public AdminSecurityRiskStory()
        {
            this._container = Shell.Instance.Container.CreateChildContainer();
        }
        [Fact]
        public void Admin_can_create_security_threat()
        {
            this._scenario = this._container.GetInstance<AdminCanCreateSecurityRiskScenario>();
            this._scenario.BDDfy();
        }
        public void Dispose()
        {
            this._container.Dispose();
            if (this._scenario != null) this._scenario.Dispose();
        }
    }
}
