using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaApp.Web.Areas.Admin.Controllers
{
    using static Common.ApplicationConstants;

    [Area(AdminRoleName)]
    [Authorize(Roles = AdminRoleName)]
    public class EventManagementController : BaseController
    {
        //private readonly IUserService userService;

        //public UserManagementController(IUserService userService)
        //{
        //    this.userService = userService;
        //}

        public IActionResult Index()
        {
            return View();
        }

        //TODO: Make Event management
    }
}

