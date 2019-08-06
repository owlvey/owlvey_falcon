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

namespace Owlvey.Falcon.IntegrationTests.Feature.Scenarios
{
    public class AdminCanCreateFeatureScenario : IDisposable
    {
        private readonly HttpClient _client;
        public AdminCanCreateFeatureScenario(HttpClient client)
        {
            _client = client;    
        }

        private FeaturePostRp representation;
        private string NewResourceLocation;

        [Given("Admin wants to create an Feature with the following attributes")]
        public void given_information()
        {
            representation = Builder<FeaturePostRp>.CreateNew()
                                 .With(x => x.Name = $"{Guid.NewGuid()}")
                                 .With(x => x.ProductId = KeyConstants.ProductId)
                                 .Build();
        }

        [When("Admin saves the new Feature")]
        public void when_send_request()
        {
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            var responsePost = _client.PostAsync($"/Features", jsonContent).Result;
            Assert.Equal(StatusCodes.Status201Created, (int)responsePost.StatusCode);
            NewResourceLocation = responsePost.Headers.Location.ToString();
        }

        [Then("Admin verifies Feature update")]
        public void then_created()
        {
            var responseGet = _client.GetAsync(NewResourceLocation).Result;
            Assert.True(responseGet.IsSuccessStatusCode);

            var FeatureRepresentation = HttpClientExtension.ParseHttpContentToModel<FeatureGetRp>(responseGet.Content);

            Assert.Equal(FeatureRepresentation.Name, representation.Name);
        }

        public void Dispose()
        {

        }
    }
}
