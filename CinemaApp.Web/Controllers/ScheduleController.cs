using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaApp.Web.Controllers
{
    [Authorize]
    public class ScheduleController : BaseController
    {
        private readonly IScheduleService scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            this.scheduleService = scheduleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Guid? userGuid = GetCurrectUserGuidId();

            if (!userGuid.HasValue)
            {
                return Unauthorized();
            }

            MyScheduleIndexViewModel viewModels = await this.scheduleService
                .GetMyScheduleByIdAsync(userGuid.Value);

            return View(viewModels);
        }
    }
}

