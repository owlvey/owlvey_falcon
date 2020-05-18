﻿using FizzWare.NBuilder;
using GST.Fake.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Owlvey.Falcon.IntegrationTests.Common;
using Owlvey.Falcon.IntegrationTests.Setup;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using TestStack.BDDfy;
using Xunit;


namespace Owlvey.Falcon.IntegrationTests.SourceItems.Scenarios
{    
    public class AdminCanCreateSourceItemProportionScenario : BaseScenario, IDisposable
    {
        private readonly HttpClient _client;
        private CustomerGetRp Customer;
        private ProductGetRp Product;
        private SourceGetRp Source;
        public AdminCanCreateSourceItemProportionScenario(HttpClient client)
        {
            _client = client;
            _client.SetFakeBearerToken(this.GetAdminToken());
        }                

        [Given("Admin creates customer and product")]
        public void given_information()
        {
            this.Customer = DataSeedUtil.CreateCustomer(this._client);
            this.Product = DataSeedUtil.CreateProduct(this._client, this.Customer);
            this.Source = DataSeedUtil.CreateSource(this._client, this.Product);
        }

        [When("Admin saves new source items")]
        public void when_send_request()
        {
            int sourceId = this.Source.Id;
            var period = DataSeedUtil.JanuaryPeriod();
            var representation = Builder<SourceItemProportionPostRp>.CreateNew()
                     .With(x => x.SourceId = sourceId)
                     .With(x => x.Start = period.start)
                     .With(x => x.End = period.end)
                     .With(x => x.Proportion = 0.9m)                     
                     .Build();

            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            var responsePost = this._client.PostAsync($"/sourceItems/proportion", jsonContent).Result;
            Assert.Equal(StatusCodes.Status200OK, (int)responsePost.StatusCode);
        }

        [Then("Admin verifies source items created")]
        public void then_created()
        {
            var responseGet = this._client.GetAsync($"/sourceItems?sourceId={this.Source.Id}").Result;
            Assert.Equal(StatusCodes.Status200OK, (int)responseGet.StatusCode);
            var responseRepresentation = HttpClientExtension.ParseHttpContentToModel<IEnumerable<SourceItemGetListRp>>(responseGet.Content);
            Assert.Equal(31, responseRepresentation.Count());
        }

        public void Dispose()
        {

        }
    }
}