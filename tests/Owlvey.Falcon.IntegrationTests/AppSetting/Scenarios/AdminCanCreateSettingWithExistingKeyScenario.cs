using FizzWare.NBuilder;
using GST.Fake.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Owlvey.Falcon.IntegrationTests.Setup;
using Owlvey.Falcon.Models;
using System;
using System.Net.Http;
using System.Text;
using TestStack.BDDfy;
using Xunit;

namespace Owlvey.Falcon.IntegrationTests.AppSetting.Scenarios
{
    public class AdminCanCreateSettingWithExistingKeyScenario : AuthenticatedScenarioBase, IDisposable
    {        
        public AdminCanCreateSettingWithExistingKeyScenario(HttpClient client): base(client)
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
            Assert.True(responsePost.IsSuccessStatusCode);
        }

        [Then("The appsetting was created")]
        public void then_created()
        {
            var responseGet = _client.GetAsync($"/appsettings/{representation.Key}").Result;
            Assert.True(responseGet.IsSuccessStatusCode);

            var appSettingRepresentation = HttpClientExtension.ParseHttpContentToModel<AppSettingPostRp>(responseGet.Content);

            Assert.Equal(appSettingRepresentation.Key, representation.Key);
            Assert.Equal(appSettingRepresentation.Value, representation.Value);
        }

        [AndThen("Admin send the request with existing key")]
        public void when_send_request_with_existing_key()
        {
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            var responsePost = _client.PostAsync($"/appsettings", jsonContent).Result;
            Assert.Equal((int)responsePost.StatusCode, StatusCodes.Status409Conflict);
        }

        public void Dispose()
        {

        }
    }
}
