using FizzWare.NBuilder;
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
    public class AdminCanUpdateCustomerScenario : IDisposable
    {
        private readonly HttpClient _client;
        public AdminCanUpdateCustomerScenario(HttpClient client)
        {
            _client = client;
        }

        private CustomerPostRp representation;
        private string NewValue = string.Empty;
        private string NewResourceLocation;

        [Given("Admin wants to create an Customer with the following attributes")]
        public void given_information()
        {
            representation = Builder<CustomerPostRp>.CreateNew()
                                 .With(x => x.Name = Faker.Company.Name())
                                 .With(x => x.Avatar = $"{Guid.NewGuid()}")
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

        [Then("The Customer was updated")]
        public void then_update()
        {
            NewValue = Faker.Company.Name();

            var representationPut = new CustomerPutRp();
            representationPut.Name = NewValue;
            representationPut.Avatar = representation.Avatar;

            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representationPut);
            var responsePut = _client.PutAsync(NewResourceLocation, jsonContent).Result;
            Assert.Equal(StatusCodes.Status200OK, (int)responsePut.StatusCode);
        }

        [AndThen("Admin send the request for get new Customer")]
        public void when_send_request_with_existing_key()
        {
            var responseGet = _client.GetAsync(NewResourceLocation).Result;
            Assert.Equal((int)responseGet.StatusCode, StatusCodes.Status200OK);

            var CustomerRepresentation = HttpClientExtension.ParseHttpContentToModel<CustomerGetRp>(responseGet.Content);

            Assert.Equal(CustomerRepresentation.Name, NewValue);
        }

        public void Dispose()
        {

        }
    }
}
