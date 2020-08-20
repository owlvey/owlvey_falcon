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
    public class AdminCanDeleteSettingScenario : AuthenticatedScenarioBase, IDisposable
    {        
        public AdminCanDeleteSettingScenario(HttpClient client) : base(client)
        {
            
        }

        private AppSettingPostRp representation;

        [Given("Admin set information")]
        public void given_information()
        {
            representation = Builder<AppSettingPostRp>.CreateNew()
                                 .With(x => x.Key = $"{Guid.NewGuid()}")
                                 .With(x => x.Value = $"{Guid.NewGuid()}")
                                 .Build();
        }

        [When("Admin send the request")]
        public void when_send_request()
        {
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            var responsePost = _client.PostAsync($"/appsettings", jsonContent).Result;
            Console.WriteLine(responsePost.Content.ReadAsStringAsync().Result);
            Assert.True(responsePost.IsSuccessStatusCode);
        }

        [Then("The appsetting delete key")]
        public void then_delete()
        {
            var responseGet = _client.DeleteAsync($"/appsettings/{representation.Key}").Result;
            Assert.True(responseGet.IsSuccessStatusCode);
        }

        [AndThen("Admin send the request with key deleted")]
        public void when_send_request_with_existing_key()
        {
            var responseGet = _client.GetAsync($"/appsettings/{representation.Key}").Result;
            Assert.Equal((int)responseGet.StatusCode, StatusCodes.Status404NotFound);
        }

        public void Dispose()
        {

        }
    }
}
