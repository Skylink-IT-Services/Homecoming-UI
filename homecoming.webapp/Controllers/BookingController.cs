using homecoming.webapp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace homecoming.webapp.Controllers
{
    public class BookingController : Controller
    {      
        public async Task<IActionResult> Accomodations(int id)
        {
            List<AccomodationViewModel> listOfAccomodationsByLocation = null;
            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.BaseUrl);
                HttpResponseMessage httpResponse = await client.GetAsync($"location/GetAccomByLocationId/{id}");
                if (httpResponse.IsSuccessStatusCode)
                {
                    listOfAccomodationsByLocation = await httpResponse.Content.ReadAsAsync<List<AccomodationViewModel>>();
                }
                else
                {
                    listOfAccomodationsByLocation = null;
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(listOfAccomodationsByLocation);
        }
    }


}
