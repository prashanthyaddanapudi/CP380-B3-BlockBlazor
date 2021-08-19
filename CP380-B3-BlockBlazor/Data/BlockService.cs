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
    public class BlockService
    {
        public HttpClient Client { get; }
        public IConfiguration Configuration { get; }
        public BlockService()
        {

        }
        public BlockService(IConfiguration config, IHttpClientFactory factory)
        {
            Configuration = config;
        }
        public async Task<IEnumerable<Block>> FetchBlocks()
        {
            IEnumerable<Block> blockList = null;

            //var httpRequestMessage = new HttpRequestMessage()
            //{
            //    Method = new HttpMethod("GET"),
            //    //RequestUri = new Uri(Configuration.GetValue<string>("BlockService:url")),
            //    RequestUri = new Uri("http://localhost:46688/blocks"),
            //};
            var client = new HttpClient();

            var response = await client.GetAsync("http://localhost:46688/blocks");
            //var response = await Client.SendAsync(httpRequestMessage);
            if (response.IsSuccessStatusCode)
            {
                string responseStream = await response.Content.ReadAsStringAsync();
                var jsonResult = JsonConvert.DeserializeObject(responseStream).ToString();
                blockList = JsonConvert.DeserializeObject<IEnumerable<Block>>(jsonResult);
               
            }
            return blockList;
        }

        public async Task<Block> SubmitNewBlockAsync(Block block)
        {
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = new HttpMethod("POST"),
                RequestUri = new Uri("http://localhost:46688/blocks"),
                Content = new StringContent(JsonConvert.SerializeObject(block), Encoding.UTF8, "application/json"),
            };
            var client = new HttpClient();
            var response = await client.SendAsync(httpRequestMessage);
            if (response.IsSuccessStatusCode)
            {
                response = await client.GetAsync("http://localhost:46688/blocks");
                if (response.IsSuccessStatusCode && response.Content != null)
                {
                    string responseStream = await response.Content.ReadAsStringAsync();
                    var jsonResult = JsonConvert.DeserializeObject(responseStream).ToString();
                    block = JsonConvert.DeserializeObject<Block>(jsonResult);
                    return block;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
    }
}
