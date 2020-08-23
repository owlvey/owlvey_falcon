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
    public class AdminCanUpdateJourneyScenario : DefaultScenarioBase, IDisposable
    {        
        public AdminCanUpdateJourneyScenario(HttpClient client): base(client)
        {
     
        }

        private JourneyPostRp representation;
        private string NewValue = "New Value";
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

        [Then("The Journey was updated")]
        public void then_update()
        {
            var representationPut = new JourneyPutRp();
            representationPut.Description = NewValue;
            
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representationPut);
            var responsePut = _client.PutAsync(NewResourceLocation, jsonContent).Result;
            Assert.Equal(StatusCodes.Status200OK, (int)responsePut.StatusCode);
        }

        [AndThen("Admin send the request for get new Journey")]
        public void when_send_request_with_existing_key()
        {
            var responseGet = _client.GetAsync(NewResourceLocation).Result;
            Assert.Equal((int)responseGet.StatusCode, StatusCodes.Status200OK);

            var JourneyRepresentation = HttpClientExtension.ParseHttpContentToModel<JourneyGetRp>(responseGet.Content);

            Assert.Equal(JourneyRepresentation.Description, NewValue);
        }

        public void Dispose()
        {

        }
    }
}
