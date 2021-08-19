using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using CP380_B1_BlockList.Models;
using System.Text;
using Newtonsoft.Json;

namespace CP380_B3_BlockBlazor.Data
{
    public class PendingTransactionService
    {
        public List<Payload> payloads { get; set; }
        public HttpClient Client { get; }
        public IConfiguration Configuration { get; }

        public PendingTransactionService()
        {

        }
        public PendingTransactionService(IConfiguration config, IHttpClientFactory factory)
        {
            Configuration = config;
        }
        public async Task<IEnumerable<Payload>> FetchPayloads()
        {
            IEnumerable<Payload> payloadList = null;
            //string url = Configuration.GetValue<string>("PayloadService:url-get");
            //var httpRequestMessage = new HttpRequestMessage()
            //{
            //    Method = new HttpMethod("GET"),
            //    RequestUri = new Uri(url),
            //    //RequestUri = new Uri("http://localhost:46688/latestPayload"),
            //};

            var client = new HttpClient();

            var response = await client.GetAsync("http://localhost:46688/latestPayload");
            //var response = await Client.SendAsync(httpRequestMessage);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                var jsonResult = JsonConvert.DeserializeObject(responseStream).ToString();
                payloadList = JsonConvert.DeserializeObject<IEnumerable<Payload>>(jsonResult);
            }
            return payloadList;
        }
        public async Task<HttpResponseMessage> AddPayload(Payload payload)
        {
            //string url = Configuration.GetValue<string>("PayloadService:url-post");
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = new HttpMethod("POST"),
                //RequestUri = new Uri(url),
                RequestUri = new Uri("http://localhost:46688/addPayload"),
                Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"),
            };
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(httpRequestMessage);
            
            return response;
        }
    }
}
