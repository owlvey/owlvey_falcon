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

namespace Owlvey.Falcon.IntegrationTests.Squad.Scenarios
{
    public class AdminCanCreateSquadScenario : IDisposable
    {
        private readonly HttpClient _client;
        public AdminCanCreateSquadScenario(HttpClient client)
        {
            _client = client;    
        }

        private SquadPostRp representation;
        private string NewResourceLocation;

        [Given("Admin wants to create an Squad with the following attributes")]
        public void given_information()
        {
            representation = Builder<SquadPostRp>.CreateNew()
                                 .With(x => x.Name = $"{Guid.NewGuid()}")
                                 .With(x => x.Description = $"{Guid.NewGuid()}")
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
        }

        [Then("Admin verifies client update")]
        public void then_created()
        {
            var responseGet = _client.GetAsync(NewResourceLocation).Result;
            Assert.True(responseGet.IsSuccessStatusCode);

            var SquadRepresentation = HttpClientExtension.ParseHttpContentToModel<SquadGetRp>(responseGet.Content);

            Assert.Equal(SquadRepresentation.Name, representation.Name);
        }

        public void Dispose()
        {

        }
    }
}
