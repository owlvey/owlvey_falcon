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
    public class AdminCanUpdateANotExistSettingScenario : BaseScenario, IDisposable
    {
        private readonly HttpClient _client;
        public AdminCanUpdateANotExistSettingScenario(HttpClient client)
        {
            _client = client;
            _client.SetFakeBearerToken(this.GetAdminToken());
        }
        
        private string Id;
        private HttpResponseMessage response;

        [Given("Admin set information")]
        public void given_information()
        {
            Id = Guid.NewGuid().ToString();
        }

        [When("Admin send the request")]
        public void when_send_request()
        {
            var representation = Builder<AppSettingPutRp>.CreateNew()
                                 .With(x => x.Value = $"{Guid.NewGuid()}")
                                 .Build();
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            response = _client.PutAsync($"/appsettings/{Id}", jsonContent).Result;
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
