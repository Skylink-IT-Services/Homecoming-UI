using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homecoming.webapp.Controllers
{
    public class BusinessUserController : Controller
    {
        public IActionResult ManageBusiness()
        {
            return View();
        }
    }
}
