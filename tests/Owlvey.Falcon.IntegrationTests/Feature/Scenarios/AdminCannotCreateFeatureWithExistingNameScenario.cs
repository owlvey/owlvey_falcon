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

namespace Owlvey.Falcon.IntegrationTests.Feature.Scenarios
{
    public class AdminCannotCreateFeatureWithExistingNameScenario : IDisposable
    {
        private readonly HttpClient _client;
        public AdminCannotCreateFeatureWithExistingNameScenario(HttpClient client)
        {
            _client = client;
        }

        private FeaturePostRp representation;
        private HttpResponseMessage responsePost;

        [Given("Admin wants to create an Feature with the following attributes")]
        public void given_information()
        {
            representation = Builder<FeaturePostRp>.CreateNew()
                                .With(x => x.Name = KeyConstants.FeatureName)
                                .With(x => x.Description = $"{Guid.NewGuid()}")
                                .With(x => x.ProductId = KeyConstants.ProductId)
                                .Build();
        }

        [When("Admin saves the new Feature")]
        public void when_send_request()
        {
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            responsePost = _client.PostAsync($"/features", jsonContent).Result;
        }

        [Then("The Feature was rejected")]
        public void then_created()
        {
            Assert.Equal(StatusCodes.Status409Conflict, (int)responsePost.StatusCode);
            
        }
        
        public void Dispose()
        {

        }
    }
}
