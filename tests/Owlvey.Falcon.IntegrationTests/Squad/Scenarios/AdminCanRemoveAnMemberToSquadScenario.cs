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
    public class AdminCanRemoveAnMemberToSquadScenario : IDisposable
    {
        private readonly HttpClient _client;
        public AdminCanRemoveAnMemberToSquadScenario(HttpClient client)
        {
            _client = client;
        }

        private SquadPostRp representation;
        private string NewResourceLocation;
        private int squadId;
        private int memberId;

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

            var memberPost = new MemberPostRp();
            memberPost.UserId = KeyConstants.UserId;
            memberPost.SquadId = squadId;

            jsonContent = HttpClientExtension.ParseModelToHttpContent(memberPost);
            responsePost = _client.PostAsync($"/members", jsonContent).Result;
            Assert.Equal(StatusCodes.Status201Created, (int)responsePost.StatusCode);

            resource = HttpClientExtension.ParseHttpContentToModel<dynamic>(responsePost.Content);
            memberId = (int)resource.id;
        }

        [Then("The Squad was updated")]
        public void then_update()
        {
            var responseDelete = _client.DeleteAsync($"/members/{memberId}").Result;
            Assert.Equal(StatusCodes.Status204NoContent, (int)responseDelete.StatusCode);
        }

        [AndThen("Admin send the request for get update Squad")]
        public void when_send_request_with_existing_key()
        {
            var responseGet = _client.GetAsync($"/members?squadId={squadId}").Result;
            Assert.Equal((int)responseGet.StatusCode, StatusCodes.Status200OK);

            var SquadRepresentation = HttpClientExtension.ParseHttpContentToList<MemberGetListRp>(responseGet.Content);

            Assert.Empty(SquadRepresentation);
        }

        public void Dispose()
        {

        }
    }
}
