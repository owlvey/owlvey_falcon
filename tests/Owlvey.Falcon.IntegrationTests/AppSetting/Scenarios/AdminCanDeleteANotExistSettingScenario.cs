using FizzWare.NBuilder;
using GST.Fake.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Owlvey.Falcon.IntegrationTests.Setup;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TestStack.BDDfy;
using Xunit;

namespace Owlvey.Falcon.IntegrationTests.AppSetting.Scenarios
{
    public class AdminCanDeleteANotExistSettingScenario : AuthenticatedScenarioBase, IDisposable
    {        
        public AdminCanDeleteANotExistSettingScenario(HttpClient client): base(client)
        {
            
        }
        
        private string Key;
        private HttpResponseMessage response;

        [Given("Admin set information")]
        public void given_information()
        {
            Key = Guid.NewGuid().ToString();
        }

        [When("Admin send the request")]
        public void when_send_request()
        {
            response = _client.DeleteAsync($"/appsettings/{Key}").Result;
        }

        [Then("The appsetting not exists")]
        public void then_created()
        {
            Assert.Equal((int)response.StatusCode, StatusCodes.Status404NotFound);
        }

        public void Dispose()
        {

        }
    }
}
