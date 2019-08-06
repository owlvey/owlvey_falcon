using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Owlvey.Falcon.IntegrationTests.Setup
{
    public static class HttpClientExtension
    {
        public static HttpContent ParseModelToHttpContent(object model)
        {
            var json = JsonConvert.SerializeObject(model);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public static T ParseHttpContentToModel<T>(HttpContent content)
        {
            string model = content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(model);
        }

        public static IReadOnlyList<T> ParseHttpContentToModelCollection<T>(HttpContent content)
        {
            string model = content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<RepresentationCollection<T>>(model).Items;
        }

        public static IReadOnlyList<T> ParseHttpContentToList<T>(HttpContent content)
        {
            string model = content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<T>>(model);
        }
    }

    public class RepresentationCollection<T>
    {
        public IReadOnlyList<T> Items { get; set; }
        public int Count { get; set; }
    }
}
