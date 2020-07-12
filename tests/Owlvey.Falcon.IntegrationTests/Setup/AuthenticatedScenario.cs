using GST.Fake.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using TestStack.BDDfy;

namespace Owlvey.Falcon.IntegrationTests.Setup
{
    public class AuthenticatedScenario: BaseScenario
    {
        public object GetAdminToken()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.sub = "C675A96B-851E-49C2-BDB8-A6BB5AA41556";
            data.name = "C675A96B-851E-49C2-BDB8-A6BB5AA41556";
            data.fullname = "admin admin";
            data.email = "admin@owlvey.com";
            data.role = new List<string>() { "admin" };
            return data;
        }

        public object GetGuestToken()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.sub = "A9EC0802-AEF9-4D72-A686-9D4C110A3B94";
            data.name = "A9EC0802-AEF9-4D72-A686-9D4C110A3B94";
            data.fullname = "admin admin";
            data.email = "admin@owlvey.com";
            data.role = new List<string>() { "admin" };

            return data;
        }
        protected readonly HttpClient _client;                
        public AuthenticatedScenario(HttpClient client) {
            _client = client;
        }
        [Given("Login", Order = -1)]
        public void on_login()
        {

            if (Shell.IsDevelopment())
            {
                _client.SetFakeBearerToken(this.GetAdminToken());
            }
            else {                

                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("scope", "api"),
                    new KeyValuePair<string, string>("client_id", "CF4A9ED44148438A99919FF285D8B48D"),
                    new KeyValuePair<string, string>("client_secret", "0da45603-282a-4fa6-a20b-2d4c3f2a2127")
                });

                var response = this._client.PostAsync(Shell.IdentityHost() + "/connect/token", formContent).Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException(response.StatusCode.ToString());
                }
                var token = HttpClientExtension.ParseHttpContentToModel<Dictionary<string,string>>(response.Content);

                _client.SetToken("Bearer", token["access_token"]);
            }
        }
    }
}
