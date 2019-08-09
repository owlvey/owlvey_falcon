using FizzWare.NBuilder;
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
    public class AdminCannotCreateServiceWithExistingNameScenario : IDisposable
    {
        private readonly HttpClient _client;
        public AdminCannotCreateServiceWithExistingNameScenario(HttpClient client)
        {
            _client = client;
        }

        private ServicePostRp representation;
        private HttpResponseMessage responsePost;

        [Given("Admin wants to create an Service with the following attributes")]
        public void given_information()
        {
            representation = Builder<ServicePostRp>.CreateNew()
                                .With(x => x.Name = KeyConstants.ServiceName)
                                .With(x => x.Description = $"{Guid.NewGuid()}")
                                .With(x => x.ProductId = KeyConstants.ProductId)
                                .With(x => x.SLO = 99)
                                .Build();
        }

        [When("Admin saves the new Service")]
        public void when_send_request()
        {
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            responsePost = _client.PostAsync($"/customers/{KeyConstants.CustomerId}/products/{KeyConstants.ProductId}/Services", jsonContent).Result;
        }

        [Then("The Service was rejected")]
        public void then_created()
        {
            Assert.Equal(StatusCodes.Status409Conflict, (int)responsePost.StatusCode);
            
        }
        
        public void Dispose()
        {

        }
    }
}