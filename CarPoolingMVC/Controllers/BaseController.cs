using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
            string baseUri = "https://localhost:5001/api/";
            //string token = Request.Cookies["Bearer"] ?? "NoValue";
            //token = token.Split(";")[0];
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            return await _httpClient.PostAsync(baseUri + absoluteUri, new StringContent(JsonConvert.SerializeObject(_object), Encoding.UTF8, "application/json"));
        }

        public HttpResponseMessage GetApi(string absoluteUri)
        {
            string baseUri = "https://localhost:5001/api/";
            //string token = Request.Cookies["Bearer"] ?? "NoValue";
            //token = token.Split(";")[0];
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            return _httpClient.GetAsync(baseUri + absoluteUri).Result;
        }
    }
}