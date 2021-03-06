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

namespace Owlvey.Falcon.IntegrationTests.Squad.Scenarios
{
    public class AdminCannotCreateSquadWithExistingNameScenario : DefaultScenarioBase, IDisposable
    {        
        public AdminCannotCreateSquadWithExistingNameScenario(HttpClient client) : base(client)
        {
     
        }

        private SquadPostRp representation;
        private HttpResponseMessage responsePost;

        [Given("Admin wants to create an Squad with the following attributes")]
        public void given_information()
        {
            representation = Builder<SquadPostRp>.CreateNew()
                                   .With(x => x.Name = KeyConstants.SquadName)                                   
                                   .With(x => x.CustomerId = this.DefaultCustomerId)
                                   .Build();
        }

        [When("Admin saves the new Squad")]
        public void when_send_request()
        {
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            responsePost = _client.PostAsync($"/Squads", jsonContent).Result;
        }

        [Then("The Squad was acapted")]
        public void then_created()
        {
            Assert.Equal(StatusCodes.Status201Created, (int)responsePost.StatusCode);
            
        }
        
        public void Dispose()
        {

        }
    }
}
