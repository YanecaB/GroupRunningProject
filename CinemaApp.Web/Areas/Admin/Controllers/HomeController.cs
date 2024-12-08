using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaApp.Web.Areas.Admin.Controllers
{
    using static Common.ApplicationConstants;

    [Area(AdminRoleName)]
    [Authorize(Roles = AdminRoleName)]
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewData["Title"] = "Admin Dashboard";
            ViewData["Message"] = "Manage the Group Running App";

            return View();
        }
    }
}

