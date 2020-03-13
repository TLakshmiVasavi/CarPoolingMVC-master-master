using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CarPoolingMVC.Controllers
{
    public class BaseController : Controller
    {
        private readonly HttpClient _httpClient;

        public BaseController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<HttpResponseMessage> RequestApi(string absoluteUri, object _object, string method)
        {
            string baseUri = "https://localhost:44302/api/";
            //using (_httpClient)
            {
                //string token = Request.Cookies["Bearer"] ?? "NoValue";
                //string token = HttpContext.Session.GetString("Bearer");
                //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                if (method == "post")
                {
                    return await _httpClient.PostAsync(baseUri + absoluteUri, new StringContent(JsonConvert.SerializeObject(_object), Encoding.UTF8, "application/json"));
                }
                else
                {
                    return _httpClient.GetAsync(baseUri + absoluteUri).Result;
                }
            }
        }
        public HttpResponseMessage GetApi(string absoluteUri)
        {
            string baseUri = "https://localhost:44302/api/";
            //using (_httpClient)
            {
                string token = Request.Cookies["Bearer"] ?? "NoValue";

                //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    return _httpClient.GetAsync(baseUri + absoluteUri).Result;
            }
        }

    }
}