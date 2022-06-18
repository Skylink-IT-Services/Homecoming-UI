using homecoming.webapp.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace homecoming.webapp.Controllers
{
    public class BasicUserController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public BasicUserController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm]BasicUserViewModel user)
        {
            if(user != null)
            {
                using(var client = new HttpClient())
                {
                    var appUser = new BasicUserViewModel()
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Cell_No = user.Cell_No,
                        Email = userManager.GetUserName(HttpContext.User),
                        Dob = user.Dob,
                        AspUserId = userManager.GetUserId(HttpContext.User)
                    };
                    client.BaseAddress = new Uri(Config.BaseUrl);
                    var json = JsonConvert.SerializeObject(appUser);
                    var payload = new StringContent(json,Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("user", payload);
                    if (response.IsSuccessStatusCode)
                    {
                        var res = response.Content.ReadAsStringAsync().Result;
                        return RedirectToAction("LandingPage","Home");
                    }
                }
            }
            return BadRequest();
        }
    }
}
