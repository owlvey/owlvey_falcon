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

namespace Owlvey.Falcon.IntegrationTests.Product.Scenarios
{
    public class AdminCanCreateProductScenario : IDisposable
    {
        private readonly HttpClient _client;
        public AdminCanCreateProductScenario(HttpClient client)
        {
            _client = client;    
        }

        private ProductPostRp representation;
        private string NewResourceLocation;

        [Given("Admin wants to create an Product with the following attributes")]
        public void given_information()
        {
            representation = Builder<ProductPostRp>.CreateNew()
                                 .With(x => x.Name = $"{Guid.NewGuid()}")
                                 .With(x => x.Description = $"{Guid.NewGuid()}")
                                 .With(x => x.CustomerId = KeyConstants.CustomerId)
                                 .Build();
        }

        [When("Admin saves the new Product")]
        public void when_send_request()
        {
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            var responsePost = _client.PostAsync($"/Products", jsonContent).Result;
            Assert.Equal(StatusCodes.Status201Created, (int)responsePost.StatusCode);
            NewResourceLocation = responsePost.Headers.Location.ToString();
        }

        [Then("Admin verifies client update")]
        public void then_created()
        {
            var responseGet = _client.GetAsync(NewResourceLocation).Result;
            Assert.True(responseGet.IsSuccessStatusCode);

            var ProductRepresentation = HttpClientExtension.ParseHttpContentToModel<ProductGetRp>(responseGet.Content);

            Assert.Equal(ProductRepresentation.Name, representation.Name);
            Assert.Equal(ProductRepresentation.Description, representation.Description);
        }

        public void Dispose()
        {

        }
    }
}
