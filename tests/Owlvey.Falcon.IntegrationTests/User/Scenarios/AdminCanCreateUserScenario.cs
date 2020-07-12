using FizzWare.NBuilder;
using GST.Fake.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Owlvey.Falcon.IntegrationTests.Setup;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TestStack.BDDfy;
using Xunit;

namespace Owlvey.Falcon.IntegrationTests.User.Scenarios
{
    public class AdminCanCreateUserScenario : AuthenticatedScenario, IDisposable
    {        
        public AdminCanCreateUserScenario(HttpClient client) : base(client)
        {
            
        }

        private UserPostRp representation;
        private string NewResourceLocation;

        [Given("Admin wants to create an User with the following attributes")]
        public void given_information()
        {
            representation = Builder<UserPostRp>.CreateNew()
                                 .With(x => x.Email = Faker.Internet.Email())
                                 .Build();
        }

        [When("Admin saves the new User")]
        public void when_send_request()
        {
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            var responsePost = _client.PostAsync($"/Users", jsonContent).Result;
            Assert.Equal(StatusCodes.Status201Created, (int)responsePost.StatusCode);
            NewResourceLocation = responsePost.Headers.Location.ToString();
        }

        [Then("Admin verifies client update")]
        public void then_created()
        {
            var responseGet = _client.GetAsync(NewResourceLocation).Result;
            Assert.True(responseGet.IsSuccessStatusCode);

            var UserRepresentation = HttpClientExtension.ParseHttpContentToModel<UserGetRp>(responseGet.Content);

            Assert.Equal(UserRepresentation.Email, representation.Email);
        }

        public void Dispose()
        {

        }
    }
}
