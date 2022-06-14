using homecoming.webapp.Models;
using homecoming.webapp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace homecoming.webapp.Controllers
{ 
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> LandingPage()
        {
            IEnumerable<BusinessViewModel> houses = null;
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.BaseUrl);
                var response = await client.GetAsync("business");
                var result = response.Content;
                if (response.IsSuccessStatusCode)
                {
                    var read = result.ReadAsAsync<IList<BusinessViewModel>>();
                    read.Wait();
                    houses = read.Result;
                }
                else
                {
                    houses = Enumerable.Empty<BusinessViewModel>();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            return View(houses);
        }
        public IActionResult AccomodationsPage(int id)
        {
           IEnumerable<AccomodationViewModel> accomodationList = null;
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.BaseUrl);
                var response = client.GetAsync($"accomodation/GetByBusinessId/{id}");
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var read = result.Content.ReadAsAsync<IList<AccomodationViewModel>>();
                    read.Wait();
                    accomodationList = read.Result;
                }
                else
                {
                    accomodationList = Enumerable.Empty<AccomodationViewModel>();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }

            }
            return View(accomodationList);
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
    }
}
