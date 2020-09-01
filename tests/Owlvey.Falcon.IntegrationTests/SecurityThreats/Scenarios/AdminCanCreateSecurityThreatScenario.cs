using FizzWare.NBuilder;
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

namespace Owlvey.Falcon.IntegrationTests.SecurityThreats.Scenarios
{
    public class AdminCanCreateSecurityThreatScenario : AuthenticatedScenarioBase, IDisposable
    {
        public AdminCanCreateSecurityThreatScenario(HttpClient client) : base(client)
        {
        }

        [Given("Admin creates threat")]
        public void given_information()
        {
            
        }

        [When("Admin send post")]
        public void when_send_request()
        {               
            var representation = Builder<SecurityThreatPostRp>.CreateNew()
                     .With(x => x.Name = "test")                     
                     .Build();

            var jsonContent = HttpClientExtension.ParseModelToHttpContent(representation);
            var responsePost = this._client.PostAsync($"/risks/security/threats", jsonContent).Result;
            Assert.Equal(StatusCodes.Status200OK, (int)responsePost.StatusCode);
        }

        [Then("Admin verifies source items created")]
        public void then_created()
        {
            var responseGet = this._client.GetAsync($"/risks/security/threats").Result;
            Assert.Equal(StatusCodes.Status200OK, (int)responseGet.StatusCode);
            var responseRepresentation = HttpClientExtension.ParseHttpContentToModel<IEnumerable<SecurityThreatGetRp>>(responseGet.Content);
            Assert.NotEmpty(responseRepresentation);
        }

        public void Dispose()
        {

        }


    }
}
