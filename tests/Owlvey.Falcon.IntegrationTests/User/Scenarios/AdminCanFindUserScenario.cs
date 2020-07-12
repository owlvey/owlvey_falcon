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
    public class AdminCanFindUserScenario : AuthenticatedScenarioBase, IDisposable
    {        
        public AdminCanFindUserScenario(HttpClient client):base(client)
        {
            
        }

        private UserPostRp representation;
        private string NewResourceLocation;

        [Given("Admin wants to find an User with the following attributes")]
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
            var responseGet = _client.GetAsync($"/Users?email={representation.Email}").Result;
            Assert.True(responseGet.IsSuccessStatusCode);

            var UserRepresentation = HttpClientExtension.ParseHttpContentToModel<IEnumerable<UserGetListRp>>(responseGet.Content);

            Assert.NotEmpty(UserRepresentation);
        }

        public void Dispose()
        {

        }
    }
}
