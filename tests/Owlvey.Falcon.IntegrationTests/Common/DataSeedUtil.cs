using FizzWare.NBuilder;
using Microsoft.AspNetCore.Http;
using Owlvey.Falcon.IntegrationTests.Customer.Scenarios;
using Owlvey.Falcon.IntegrationTests.Setup;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Owlvey.Falcon.IntegrationTests.Common
{
    internal class DataSeedUtil
    {
        internal static CustomerGetRp CreateCustomer(in HttpClient client)
        {
            var representation = Builder<CustomerPostRp>.CreateNew()
                     .With(x => x.Name = Faker.Company.Name())
                     .Build();
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            var responsePost = client.PostAsync($"/Customers", jsonContent).Result;
            Assert.Equal(StatusCodes.Status201Created, (int)responsePost.StatusCode);
            var NewResourceLocation = responsePost.Headers.Location.ToString();
            var responseGet = client.GetAsync(NewResourceLocation).Result;
            var CustomerRepresentation = HttpClientExtension.ParseHttpContentToModel<CustomerGetRp>(responseGet.Content);
            return CustomerRepresentation;
        }

        internal static ProductGetRp CreateProduct(in HttpClient client, in CustomerGetRp customer) {
            int customerId = customer.Id;
            var representation = Builder<ProductPostRp>.CreateNew()
                     .With(x => x.Name = $"{Guid.NewGuid()}")
                     .With(x => x.CustomerId = customerId)
                     .Build();

            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            var responsePost = client.PostAsync($"/products", jsonContent).Result;
            Assert.Equal(StatusCodes.Status201Created, (int)responsePost.StatusCode);
            var NewResourceLocation = responsePost.Headers.Location.ToString();
            var responseGet = client.GetAsync(NewResourceLocation).Result;
            var ProductRepresentation = HttpClientExtension.ParseHttpContentToModel<ProductGetRp>(responseGet.Content);
            return ProductRepresentation;
        }

        internal static SourceGetRp CreateSource(in HttpClient client, in ProductGetRp product)
        {
            int productId = product.Id;
            var representation = Builder<SourcePostRp>.CreateNew()
                     .With(x => x.Name = $"{Guid.NewGuid()}")
                     .With(x => x.ProductId = productId)                     
                     .Build();

            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            var responsePost = client.PostAsync($"/sources", jsonContent).Result;
            Assert.Equal(StatusCodes.Status201Created, (int)responsePost.StatusCode);
            var NewResourceLocation = responsePost.Headers.Location.ToString();
            var responseGet = client.GetAsync(NewResourceLocation).Result;
            var responseRepresentation = HttpClientExtension.ParseHttpContentToModel<SourceGetRp>(responseGet.Content);
            return responseRepresentation;
        }

        internal static SecurityThreatGetRp CreateSecurityThreat(in HttpClient client)
        {            
            var representation = Builder<SecurityThreatPostRp>.CreateNew()
                     .With(x => x.Name = $"{Guid.NewGuid()}")                     
                     .Build();

            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            var responsePost = client.PostAsync($"/risks/security/threats", jsonContent).Result;
            Assert.Equal(StatusCodes.Status200OK, (int)responsePost.StatusCode);
            var responseRepresentation = HttpClientExtension.ParseHttpContentToModel<SecurityThreatGetRp>(responsePost.Content);
            return responseRepresentation;
        }
        internal static (DateTime start, DateTime end) JanuaryPeriod() {
            return (new DateTime(2019, 1, 1), new DateTime(2019, 1, 31));
        }
    }
}
