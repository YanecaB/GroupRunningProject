using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaApp.Services.Data;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Event;
using CinemaApp.Web.ViewModels.Group;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaApp.Web.Controllers
{
    public class EventController : BaseController
    {
        private readonly IEventService eventService;

        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        public async Task<IActionResult> Index()
        {
            var events = await eventService.IndexGetAllAsync();

            return this.View(events);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create(string? id)
        {
            Guid guidId = Guid.Empty;
            bool isIdValid = this.IsGuidValid(id, ref guidId);
            if (!isIdValid)
            {
                return this.RedirectToAction(nameof(Index));
            }

            var model = new EventCreateViewModel
            {
                GroupId = guidId.ToString()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(EventCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Guid? userGuid = GetCurrectUserGuidId();

            if (!userGuid.HasValue)
            {
                return Unauthorized();
            }

            await eventService.AddEventAsync(model, userGuid.Value);

            return RedirectToAction(nameof(Admin));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Admin()
        {
            Guid? userGuid = GetCurrectUserGuidId();

            if (!userGuid.HasValue)
            {
                return Unauthorized();
            }

            IEnumerable<EventIndexViewModel> events =
               await this.eventService.GetAllAdminEventsAsync(userGuid.Value);

            return this.View(events);
        }

        public async Task<IActionResult> Join(string? id)
        {
            Guid guidId = Guid.Empty;
            bool isIdValid = this.IsGuidValid(id, ref guidId);
            if (!isIdValid)
            {
                return this.RedirectToAction(nameof(Index));
            }

            Guid? userGuid = GetCurrectUserGuidId();

            if (!userGuid.HasValue)
            {
                return Unauthorized();
            }

            await this.eventService.JoinEventAsync(guidId, userGuid.Value);

            return RedirectToAction(nameof(Index)); // todo: redirect to My schedule

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(string? id)
        {
            Guid guidId = Guid.Empty;
            bool isIdValid = this.IsGuidValid(id, ref guidId);
            if (!isIdValid)
            {
                return this.RedirectToAction(nameof(Index));
            }

            Guid? userGuid = GetCurrectUserGuidId();

            if (!userGuid.HasValue)
            {
                return Unauthorized();
            }

            EventDetailsViewModel viewModel =
                await this.eventService.GetEventDetailsByIdAsync(guidId, userGuid.Value);

            return this.View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string? id)
        {
            Guid guidId = Guid.Empty;
            bool isIdValid = this.IsGuidValid(id, ref guidId);
            if (!isIdValid)
            {
                return this.RedirectToAction(nameof(Index));
            }

            EventEditViewModel? viewModel = await this.eventService
               .GetEventForEditAsync(guidId);

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EventEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            bool isUpdated = await this.eventService
               .EditEventAsync(viewModel);

            if (!isUpdated)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error occurred while updating the event! Please contact administrator");
                return this.View(viewModel);
            }

            return RedirectToAction(nameof(Admin));
        }
    }
}

