using FizzWare.NBuilder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Owlvey.Falcon.IntegrationTests.Constants;
using Owlvey.Falcon.IntegrationTests.Setup;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TestStack.BDDfy;
using Xunit;

namespace Owlvey.Falcon.IntegrationTests.Service.Scenarios
{
    public class AdminCanDeleteServiceScenario : IDisposable
    {
        private readonly HttpClient _client;
        public AdminCanDeleteServiceScenario(HttpClient client)
        {
            _client = client;
        }

        private ServicePostRp representation;
        private string NewResourceLocation;

        [Given("Admin wants to create an Service with the following attributes")]
        public void given_information()
        {
            representation = Builder<ServicePostRp>.CreateNew()
                                 .With(x => x.Name = $"{Guid.NewGuid()}")                                 
                                 .With(x => x.ProductId = KeyConstants.ProductId)                                 
                                 .Build();
        }

        [When("Admin saves the new Service")]
        public void when_send_request()
        {
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            var responsePost = _client.PostAsync($"/services", jsonContent).Result;
            Assert.Equal(StatusCodes.Status201Created, (int)responsePost.StatusCode);
            NewResourceLocation = responsePost.Headers.Location.ToString();
        }

        [Then("The Service was deleted")]
        public void then_delete()
        {
            var responseGet = _client.DeleteAsync(NewResourceLocation).Result;
            Assert.True(responseGet.IsSuccessStatusCode);
        }

        [AndThen("Administrator verifies client deletion")]
        public void when_send_request_with_existing_key()
        {
            var responseGet = _client.GetAsync(NewResourceLocation).Result;
            Assert.Equal(StatusCodes.Status404NotFound, (int)responseGet.StatusCode);
        }

        public void Dispose()
        {

        }
    }
}
