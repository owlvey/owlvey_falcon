using FizzWare.NBuilder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Owlvey.Falcon.IntegrationTests.Constants;
using Owlvey.Falcon.IntegrationTests.Setup;
using Owlvey.Falcon.Models;
using System;
using System.Net.Http;
using System.Text;
using TestStack.BDDfy;
using Xunit;

namespace Owlvey.Falcon.IntegrationTests.Product.Scenarios
{
    public class AdminCannotCreateProductWithExistingNameScenario : IDisposable
    {
        private readonly HttpClient _client;
        public AdminCannotCreateProductWithExistingNameScenario(HttpClient client)
        {
            _client = client;
        }

        private ProductPostRp representation;
        private HttpResponseMessage responsePost;

        [Given("Admin wants to create an Product with the following attributes")]
        public void given_information()
        {
            representation = Builder<ProductPostRp>.CreateNew()
                                 .With(x => x.Name = KeyConstants.ProductName)
                                 .With(x => x.Description = $"{Guid.NewGuid()}")
                                 .With(x => x.CustomerId = KeyConstants.CustomerId)
                                 .Build();
        }

        [When("Admin saves the new Product")]
        public void when_send_request()
        {
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            responsePost = _client.PostAsync($"/products", jsonContent).Result;
        }

        [Then("The Product was rejected")]
        public void then_created()
        {
            Assert.Equal(StatusCodes.Status409Conflict, (int)responsePost.StatusCode);
            
        }
        
        public void Dispose()
        {

        }
    }
}
