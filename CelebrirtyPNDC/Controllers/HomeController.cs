using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace CurrencyConverter.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", "AwcbuoOyyV3J0kDVtQqSWg==BItpvlVGu2wuTrgd");
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetCelebrityDetails(string celebrityName)
        {
            try
            {
                string url = $"https://api.api-ninjas.com/v1/celebrity?name={celebrityName}";

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                JToken responseData = JToken.Parse(responseBody);

                if (responseData is JArray dataArray && dataArray.Count > 0)
                {
                    JObject celebrityDetails = dataArray[0] as JObject;

                    if (celebrityDetails != null)
                    {
                        string name = celebrityDetails.Value<string>("name");
                        decimal netWorth = celebrityDetails.Value<decimal>("net_worth");
                        string gender = celebrityDetails.Value<string>("gender");
                        string nationality = celebrityDetails.Value<string>("nationality");
                        List<string> occupations = celebrityDetails["occupation"]?.ToObject<List<string>>() ?? new List<string>();
                        decimal height = celebrityDetails.Value<decimal>("height");
                        string birthday = celebrityDetails.Value<string>("birthday");

                        return Json(new
                        {
                            success = true,
                            name = name,
                            netWorth = netWorth,
                            gender = gender,
                            nationality = nationality,
                            occupations = occupations,
                            height = height,
                            birthday = birthday
                        });
                    }
                }

                return Json(new
                {
                    success = false,
                    message = "Celebrity not found or invalid response format."
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "An error occurred, please try again later."
                });
            }
        }
    }
}
