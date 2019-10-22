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
    public class AdminCanCreateCustomerScenario : BaseScenario, IDisposable
    {
        private readonly HttpClient _client;
        public AdminCanCreateCustomerScenario(HttpClient client)
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

        [Then("Admin verifies client update")]
        public void then_created()
        {
            var responseGet = _client.GetAsync(NewResourceLocation).Result;
            Assert.True(responseGet.IsSuccessStatusCode);

            var CustomerRepresentation = HttpClientExtension.ParseHttpContentToModel<CustomerGetRp>(responseGet.Content);

            Assert.Equal(CustomerRepresentation.Name, representation.Name);
        }

        public void Dispose()
        {

        }
    }
}
