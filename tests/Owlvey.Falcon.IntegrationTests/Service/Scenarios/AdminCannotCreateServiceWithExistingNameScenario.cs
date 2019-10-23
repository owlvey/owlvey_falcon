using FizzWare.NBuilder;
using GST.Fake.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Owlvey.Falcon.IntegrationTests.Constants;
using Owlvey.Falcon.IntegrationTests.Setup;
using Owlvey.Falcon.Models;
using System;
using System.Net.Http;
using System.Text;
using TestStack.BDDfy;
using Xunit;

namespace Owlvey.Falcon.IntegrationTests.Service.Scenarios
{
    public class AdminCannotCreateServiceWithExistingNameScenario : BaseScenario, IDisposable
    {
        private readonly HttpClient _client;
        public AdminCannotCreateServiceWithExistingNameScenario(HttpClient client)
        {
            _client = client;
            _client.SetFakeBearerToken(this.GetAdminToken());
        }

        private ServicePostRp representation;
        private HttpResponseMessage responsePost;

        [Given("Admin wants to create an Service with the following attributes")]
        public void given_information()
        {
            representation = Builder<ServicePostRp>.CreateNew()
                                .With(x => x.Name = KeyConstants.ServiceName)                                
                                .With(x => x.ProductId = KeyConstants.ProductId)                                
                                .Build();
        }

        [When("Admin saves the new Service")]
        public void when_send_request()
        {
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            responsePost = _client.PostAsync($"/services", jsonContent).Result;
        }

        [Then("The Service was accepted")]
        public void then_created()
        {
            Assert.Equal(StatusCodes.Status201Created, (int)responsePost.StatusCode);
            
        }
        
        public void Dispose()
        {

        }
    }
}
