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

namespace Owlvey.Falcon.IntegrationTests.Squad.Scenarios
{
    public class AdminCannotCreateSquadWithExistingNameScenario : IDisposable
    {
        private readonly HttpClient _client;
        public AdminCannotCreateSquadWithExistingNameScenario(HttpClient client)
        {
            _client = client;
        }

        private SquadPostRp representation;
        private HttpResponseMessage responsePost;

        [Given("Admin wants to create an Squad with the following attributes")]
        public void given_information()
        {
            representation = Builder<SquadPostRp>.CreateNew()
                                   .With(x => x.Name = KeyConstants.SquadName)                                   
                                   .With(x => x.CustomerId = KeyConstants.CustomerId)
                                   .Build();
        }

        [When("Admin saves the new Squad")]
        public void when_send_request()
        {
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            responsePost = _client.PostAsync($"/Squads", jsonContent).Result;
        }

        [Then("The Squad was rejected")]
        public void then_created()
        {
            Assert.Equal(StatusCodes.Status409Conflict, (int)responsePost.StatusCode);
            
        }
        
        public void Dispose()
        {

        }
    }
}
