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

namespace Owlvey.Falcon.IntegrationTests.Journey.Scenarios
{
    public class AdminCannotCreateJourneyWithExistingNameScenario : DefaultScenarioBase, IDisposable
    {        
        public AdminCannotCreateJourneyWithExistingNameScenario(HttpClient client): base(client)
        {
            
        }

        private JourneyPostRp representation;
        private HttpResponseMessage responsePost;

        [Given("Admin wants to create an Journey with the following attributes")]
        public void given_information()
        {
            representation = Builder<JourneyPostRp>.CreateNew()
                                .With(x => x.Name = KeyConstants.JourneyName)                                
                                .With(x => x.ProductId = this.DefaultProductId)                                
                                .Build();
        }

        [When("Admin saves the new Journey")]
        public void when_send_request()
        {
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            responsePost = _client.PostAsync($"/journeys", jsonContent).Result;
        }

        [Then("The Journey was accepted")]
        public void then_created()
        {
            Assert.Equal(StatusCodes.Status201Created, (int)responsePost.StatusCode);
            
        }
        
        public void Dispose()
        {

        }
    }
}
