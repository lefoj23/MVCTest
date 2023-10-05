using Microsoft.AspNetCore.Mvc;
using MVCTest.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;

namespace MVCTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

        }

        public IActionResult Index()
        {
            ViewData["WebAPIHost"] = _configuration.GetValue<string>("WebAPIHost");

            return View();
        }

        public async Task<IActionResult> UpdateAsync([FromRoute]Guid id)
        {
            ViewData["WebAPIHost"] = _configuration.GetValue<string>("WebAPIHost");

            ViewData["isEditing"] = true;
            ViewData["employeeId"] = id;

            ViewData["employeeDetails"] = await FetchEmployeeDetailsAsync(id);

            return View("EmployeeDetails");
        }

        public async Task<IActionResult> ViewAsync([FromRoute] Guid id)
        {
            ViewData["WebAPIHost"] = _configuration.GetValue<string>("WebAPIHost");

            ViewData["isEditing"] = false;
            ViewData["employeeId"] = id;
            ViewData["employeeDetails"] = await FetchEmployeeDetailsAsync(id);

            return View("EmployeeDetails");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<EmployeeViewModel?> FetchEmployeeDetailsAsync(Guid employeeId)
        {
            var webApiHost = _configuration.GetValue<string>("WebAPIHost");
            var result = new EmployeeViewModel();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string apiEndpoint = webApiHost + "/api/Employee/" + employeeId.ToString();

                    HttpResponseMessage response = await httpClient.GetAsync(apiEndpoint);

                    if (!response.IsSuccessStatusCode)
                        throw new Exception("Something went wrong.");

                    // Parse and process the response here
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(responseBody))
                        throw new Exception("Employee Details not found!");

                    var employeeDetails = JsonConvert.DeserializeObject<EmployeeViewModel>(responseBody);

                    result = employeeDetails ?? null;

                }
                return result;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}