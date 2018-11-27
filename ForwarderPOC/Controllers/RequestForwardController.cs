using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ForwarderPOC.Controllers
{

    public class RequestBody
    {
        public string url { get; set; }
        public Object body { get; set; }
        public string method { get; set; }
    }
    public class Client
    {
        static HttpClient client = new HttpClient();

        public static async Task<HttpResponseMessage> GetTask(string requestUrl)
        {
            return await client.GetAsync(requestUrl);
        }

        static public async Task<HttpResponseMessage> PostTask(string requestUrl, object body)
        {

            var strContent = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
            return await client.PostAsync(requestUrl, strContent);
        }

        static public async Task<string> HandleResponse(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }
    }

    [Route("api/forward")]
    [ApiController]
    public class RequestForwardingController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<string> Post([FromBody] RequestBody obj)
        {
            Console.WriteLine("Testing");
            Console.WriteLine(obj.method);
            switch (obj.method) {
                case "GET":
                    return await Client.HandleResponse(await Client.GetTask(obj.url));
                case "POST":
                    return await Client.HandleResponse(await Client.PostTask(obj.url, obj.body));
                default:
                    return null;
            }

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] int value)
        {
        }

        [HttpPost("testing")]
        public ActionResult<string> testing([FromBody] string body) {
            Console.WriteLine("yay testing called");
            return body;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
