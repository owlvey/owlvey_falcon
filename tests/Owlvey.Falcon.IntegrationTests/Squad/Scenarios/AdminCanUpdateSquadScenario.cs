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
    public class AdminCanUpdateSquadScenario : IDisposable
    {
        private readonly HttpClient _client;
        public AdminCanUpdateSquadScenario(HttpClient client)
        {
            _client = client;
        }

        private SquadPostRp representation;
        private string NewValue = "New Value";
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

        [Then("The Squad was updated")]
        public void then_update()
        {
            var representationPut = new SquadPutRp();
            representationPut.Name = NewValue;
            representationPut.Description = NewValue;

            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representationPut);
            var responsePut = _client.PutAsync(NewResourceLocation, jsonContent).Result;
            Assert.Equal(StatusCodes.Status200OK, (int)responsePut.StatusCode);
        }

        [AndThen("Admin send the request for get new Squad")]
        public void when_send_request_with_existing_key()
        {
            var responseGet = _client.GetAsync(NewResourceLocation).Result;
            Assert.Equal((int)responseGet.StatusCode, StatusCodes.Status200OK);

            var SquadRepresentation = HttpClientExtension.ParseHttpContentToModel<SquadGetRp>(responseGet.Content);

            Assert.Equal(SquadRepresentation.Name, NewValue);
        }

        public void Dispose()
        {

        }
    }
}
