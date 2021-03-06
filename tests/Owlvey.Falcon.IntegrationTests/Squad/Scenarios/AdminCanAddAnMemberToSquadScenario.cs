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

namespace Owlvey.Falcon.IntegrationTests.Squad.Scenarios
{
    public class AdminCanAddAnMemberToSquadScenario : DefaultScenarioBase, IDisposable
    {
        
        public AdminCanAddAnMemberToSquadScenario(HttpClient client) : base(client)
        {
        
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
                                 .With(x => x.CustomerId = this.DefaultCustomerId)
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

        [Then("The Squad was updated")]
        public void then_update()
        {
            var memberPost = new MemberPostRp();
            memberPost.UserId = this.DefaultMemberId;
            memberPost.SquadId = squadId;

            var jsonContent = HttpClientExtension.ParseModelToHttpContent(memberPost);
            var responsePost = this._client.PostAsync($"/members", jsonContent).Result;
            Assert.Equal(StatusCodes.Status201Created, (int)responsePost.StatusCode);
        }

        [AndThen("Admin send the request for get update Squad")]
        public void when_send_request_with_existing_key()
        {
            var responseGet = _client.GetAsync($"/members?squadId={squadId}").Result;
            Assert.Equal((int)responseGet.StatusCode, StatusCodes.Status200OK);
            var SquadRepresentation = HttpClientExtension.ParseHttpContentToList<MemberGetListRp>(responseGet.Content);
            Assert.NotEmpty(SquadRepresentation);
        }

        public void Dispose()
        {

        }
    }
}
