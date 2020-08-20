using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
using Owlvey.Falcon.IntegrationTests.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using TestStack.BDDfy;

namespace Owlvey.Falcon.IntegrationTests.Setup
{
    public class DefaultScenarioBase: AuthenticatedScenarioBase
    {        
        protected int DefaultCustomerId { get; set; }
        protected int DefaultProductId { get; set; }
        protected int DefaultSquadId { get; set; }
        protected int DefaultMemberId { get; set; }
        public DefaultScenarioBase(HttpClient client): base(client)
        {
            
        }
        [Given("default customer", Order = -99)]
        public virtual void GivenDefaultCustomer()
        {
            
            var responseCustomers = this._client.GetAsync("/customers/lite").Result;
            var customers = HttpClientExtension.ParseHttpContentToList<Dictionary<string, object>>(responseCustomers.Content);
            var defaultCustomer = customers.FirstOrDefault(c => c.Where(d => d.Key == "name" && (string)d.Value == "Default Customer").Count() > 0);
            if (defaultCustomer == null)
            {
                var jsonContent = HttpClientExtension.ParseModelToHttpContent(new
                {
                    Name =  "Default Customer"
                });
                var result = this._client.PostAsync("/customers", jsonContent).Result;
                if (!result.IsSuccessStatusCode)
                    throw new ApplicationException(result.Content.ReadAsStringAsync().Result);

                var customerModel = HttpClientExtension.ParseHttpContentToModel<Dictionary<string, object>>(result.Content);
                this.DefaultCustomerId = Convert.ToInt32(customerModel["id"]);
            }
            else
            {
                this.DefaultCustomerId = Convert.ToInt32(defaultCustomer["id"]);
            }            
        }


        [Given("default user", Order = -99)]
        public virtual void GivenDefaultUser()
        {            
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(new
            {
                email = KeyConstants.UserEmail,                    
            });
            var result = this._client.PostAsync("/users", jsonContent).Result;
            if (!result.IsSuccessStatusCode)
                throw new ApplicationException(result.Content.ReadAsStringAsync().Result);

            var model = HttpClientExtension.ParseHttpContentToModel<Dictionary<string, object>>(result.Content);
            this.DefaultMemberId = Convert.ToInt32(model["id"]);            
        }


        [Given("default product", Order = -98)]
        public virtual void GivenDefaultProduct() {            
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(new
            {
                Name = KeyConstants.ProductName,
                customerId = this.DefaultCustomerId
            });
            var result = this._client.PostAsync("/products", jsonContent).Result;
            if (!result.IsSuccessStatusCode)
                throw new ApplicationException(result.Content.ReadAsStringAsync().Result);

            var model = HttpClientExtension.ParseHttpContentToModel<Dictionary<string, object>>(result.Content);
            this.DefaultProductId = Convert.ToInt32(model["id"]);            
        }
        [Given("default feature", Order = -98)]
        public virtual void GivenDefaultFeature()
        {            
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(new
            {
                Name = KeyConstants.FeatureName,
                productId = this.DefaultProductId
            });
            var result = this._client.PostAsync("/features", jsonContent).Result;
            if (!result.IsSuccessStatusCode)
                throw new ApplicationException(result.Content.ReadAsStringAsync().Result);
            
        }
        [Given("default service", Order = -98)]
        public virtual void GivenDefaultService()
        {            
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(new
            {
                Name = KeyConstants.ServiceName,
                productId = this.DefaultProductId
            });
            var result = this._client.PostAsync("/services", jsonContent).Result;
            if (!result.IsSuccessStatusCode)
                throw new ApplicationException(result.Content.ReadAsStringAsync().Result);
            
        }

        [Given("default squad", Order = -97)]
        public virtual void GivenDefaultSquad()
        {            
            var jsonContent = HttpClientExtension.ParseModelToHttpContent(new
            {
                Name = KeyConstants.SquadName,
                customerId = this.DefaultCustomerId
            });
            var result = this._client.PostAsync("/squads", jsonContent).Result;
            if (!result.IsSuccessStatusCode)
                throw new ApplicationException(result.Content.ReadAsStringAsync().Result);
            var model = HttpClientExtension.ParseHttpContentToModel<Dictionary<string, object>>(result.Content);
            this.DefaultSquadId = Convert.ToInt32(model["id"]);              
        }
    }
}
