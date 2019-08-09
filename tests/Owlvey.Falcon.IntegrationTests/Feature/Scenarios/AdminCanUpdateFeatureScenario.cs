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
    public class AdminCanUpdateFeatureScenario : IDisposable
    {
        private readonly HttpClient _client;
        public AdminCanUpdateFeatureScenario(HttpClient client)
        {
            _client = client;
        }

        private FeaturePostRp representation;
        private string NewValue = "New Value";
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
            var responsePost = _client.PostAsync($"/features", jsonContent).Result;
            Assert.Equal(StatusCodes.Status201Created, (int)responsePost.StatusCode);
            NewResourceLocation = responsePost.Headers.Location.ToString();
        }

        [Then("The Feature was updated")]
        public void then_update()
        {
            var representationPut = new FeaturePutRp();
            representationPut.Name = NewValue;
            representationPut.Description = NewValue;

            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representationPut);
            var responsePut = _client.PutAsync(NewResourceLocation, jsonContent).Result;
            Assert.Equal(StatusCodes.Status200OK, (int)responsePut.StatusCode);
        }

        [AndThen("Admin send the request for get new Feature")]
        public void when_send_request_with_existing_key()
        {
            var responseGet = _client.GetAsync(NewResourceLocation).Result;
            Assert.Equal((int)responseGet.StatusCode, StatusCodes.Status200OK);

            var FeatureRepresentation = HttpClientExtension.ParseHttpContentToModel<FeatureGetRp>(responseGet.Content);

            Assert.Equal(FeatureRepresentation.Name, NewValue);
        }

        public void Dispose()
        {

        }
    }
}
