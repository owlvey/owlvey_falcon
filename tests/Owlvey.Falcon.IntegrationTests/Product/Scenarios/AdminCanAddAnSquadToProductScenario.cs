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

namespace Owlvey.Falcon.IntegrationTests.Product.Scenarios
{
    public class AdminCanAddAnSquadToProductScenario : IDisposable
    {
        private readonly HttpClient _client;
        public AdminCanAddAnSquadToProductScenario(HttpClient client)
        {
            _client = client;
        }

        private SquadPostRp representation;
        private string NewValue = "New Value";
        private string NewResourceLocation;
        private int squadId;

        [Given("Admin wants to create an Squad with the following attributes")]
        public void given_information()
        {
            representation = Builder<SquadPostRp>.CreateNew()
                                 .With(x => x.Name = $"{Guid.NewGuid()}")                                 
                                 .With(x => x.CustomerId = KeyConstants.CustomerId)
                                 .Build();
        }

        [When("Admin saves the new Squad")]
        public void when_send_request()
        {
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            var responsePost = _client.PostAsync($"/Squads", jsonContent).Result;
            Assert.Equal(StatusCodes.Status201Created, (int)responsePost.StatusCode);
            NewResourceLocation = responsePost.Headers.Location.ToString();
            var resource = HttpClientExtension.ParseHttpContentToModel<dynamic>(responsePost.Content);
            squadId = (int)resource.id;
            
        }

        [Then("The SquadProduct was updated")]
        public void then_update()
        {
            var memberPost = new SquadProductPostRp();
            memberPost.ProductId = KeyConstants.ProductId;
            memberPost.SquadId = squadId;

            var jsonContent = HttpClientExtension.ParseModelToHttpContent(memberPost);
            var responsePost = this._client.PostAsync($"/squadProducts", jsonContent).Result;
            Assert.Equal(StatusCodes.Status201Created, (int)responsePost.StatusCode);
        }

        [AndThen("Admin send the request for get update Squad")]
        public void when_send_request_with_existing_key()
        {
            var responseGet = _client.GetAsync($"/squadProducts?id={squadId}").Result;
            Assert.Equal((int)responseGet.StatusCode, StatusCodes.Status200OK);
            var SquadRepresentation = HttpClientExtension.ParseHttpContentToList<SquadProductGetRp>(responseGet.Content);
            Assert.NotNull(SquadRepresentation);
        }

        public void Dispose()
        {

        }
    }
}
