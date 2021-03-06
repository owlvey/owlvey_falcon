using FizzWare.NBuilder;
using GST.Fake.Authentication.JwtBearer;
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

namespace Owlvey.Falcon.IntegrationTests.Journey.Scenarios
{
    public class AdminCanCreateJourneyScenario : DefaultScenarioBase, IDisposable
    {        
        public AdminCanCreateJourneyScenario(HttpClient client) : base(client)
        {            

        }

        private JourneyPostRp representation;
        private string NewResourceLocation;

        [Given("Admin wants to create an Journey with the following attributes")]
        public void given_information()
        {
            representation = Builder<JourneyPostRp>.CreateNew()
                                 .With(x => x.Name = $"{Guid.NewGuid()}")                                 
                                 .With(x => x.ProductId = this.DefaultProductId)                                 
                                 .Build();
        }

        [When("Admin saves the new Journey")]
        public void when_send_request()
        {
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            var responsePost = _client.PostAsync($"/journeys", jsonContent).Result;
            Assert.Equal(StatusCodes.Status201Created, (int)responsePost.StatusCode);
            NewResourceLocation = responsePost.Headers.Location.ToString();
        }

        [Then("Admin verifies client update")]
        public void then_created()
        {
            var responseGet = _client.GetAsync(NewResourceLocation).Result;
            Assert.True(responseGet.IsSuccessStatusCode);

            var JourneyRepresentation = HttpClientExtension.ParseHttpContentToModel<JourneyGetRp>(responseGet.Content);

            Assert.Equal(JourneyRepresentation.Name, representation.Name);
        }

        public void Dispose()
        {

        }
    }
}
