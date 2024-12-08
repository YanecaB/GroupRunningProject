using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaApp.Services.Data;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaApp.Web.Controllers
{
    [Authorize]
    public class ScheduleController : BaseController
    {
        private readonly IScheduleService scheduleService;
        private readonly IEventService eventService;

        public ScheduleController(IScheduleService scheduleService,
            IEventService eventService)
        {
            this.scheduleService = scheduleService;
            this.eventService = eventService;
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

        [HttpPost]
        public async Task<IActionResult> Remove(string? eventId)
        {
            Guid guidId = Guid.Empty;
            bool isIdValid = this.IsGuidValid(eventId, ref guidId);
            if (!isIdValid)
            {
                return this.RedirectToAction(nameof(Index));
            }

            Guid? userGuid = GetCurrectUserGuidId();

            if (!userGuid.HasValue)
            {
                return Unauthorized();
            }

            var result = await this.eventService.RemoveAnttendeeAsync(guidId, userGuid.Value);

            return RedirectToAction(nameof(Index));
        }
    }
}

