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

namespace Owlvey.Falcon.IntegrationTests.Customer.Scenarios
{
    public class AdminCanDeleteCustomerScenario : BaseScenario, IDisposable
    {
        private readonly HttpClient _client;
        public AdminCanDeleteCustomerScenario(HttpClient client)
        {
            _client = client;
            _client.SetFakeBearerToken(this.GetAdminToken());
        }

        private CustomerPostRp representation;
        private string NewResourceLocation;

        [Given("Admin wants to create an Customer with the following attributes")]
        public void given_information()
        {
            representation = Builder<CustomerPostRp>.CreateNew()
                                 .With(x => x.Name = Faker.Company.Name())                                 
                                 .Build();
        }

        [When("Admin saves the new Customer")]
        public void when_send_request()
        {
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            var responsePost = _client.PostAsync($"/Customers", jsonContent).Result;
            Assert.Equal(StatusCodes.Status201Created, (int)responsePost.StatusCode);
            NewResourceLocation = responsePost.Headers.Location.ToString();
        }

        [Then("The Customer was deleted")]
        public void then_delete()
        {
            var responseGet = _client.DeleteAsync(NewResourceLocation).Result;
            Assert.True(responseGet.IsSuccessStatusCode);
        }

        [AndThen("Administrator verifies client deletion")]
        public void when_send_request_with_existing_key()
        {
            var responseGet = _client.GetAsync(NewResourceLocation).Result;
            Assert.Equal(StatusCodes.Status404NotFound, (int)responseGet.StatusCode);
        }

        public void Dispose()
        {

        }
    }
}
