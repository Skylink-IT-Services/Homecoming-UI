using homecoming.webapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homecoming.webapp.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> manager;

        public AdminController(RoleManager<IdentityRole> manager)
        {
            this.manager = manager;
        }

        public IActionResult CreateRole()
        {
             return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(UserRole role)
        {
            var roleExist = await manager.RoleExistsAsync(role.Rolename);
            if (!roleExist)
            {
                await manager.CreateAsync(new IdentityRole(role.Rolename));
            }
            return RedirectToAction();
        }
    }
}
