using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaApp.Services.Data;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Event;
using CinemaApp.Web.ViewModels.Group;
using CinemaApp.Web.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        [HttpGet]
        public async Task<IActionResult> Index(string? searchQuery = null, int pageNumber = 1)
        {
            IEnumerable<EventIndexViewModel> events = null!;
            int totalPages = 0;
            
            if (User.Identity.IsAuthenticated)
            {
                Guid? userGuid = GetCurrectUserGuidId();

                if (!userGuid.HasValue)
                {
                    return Unauthorized();
                }

                (events, totalPages) = await eventService
                .IndexGetAllAsync(userGuid.Value, searchQuery, pageNumber);
            }
            else
            {
                (events, totalPages) = await eventService
                .IndexGetAllAsync(null, searchQuery, pageNumber);
            }

            ViewData["SearchQuery"] = searchQuery;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;
            ViewData["CurrentPage"] = pageNumber;

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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Join(string? id)
        {
            Guid guidId = Guid.Empty;
            bool isIdValid = this.IsGuidValid(id, ref guidId);
            if (!isIdValid)
            {
                return this.RedirectToAction(nameof(Index));
            }

            if (!await this.eventService.EventExistsByIdAsync(guidId))
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

            if (!await this.eventService.EventExistsByIdAsync(guidId))
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

            if (!await this.eventService.EventExistsByIdAsync(guidId))
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveAttendee(string eventId, string attendeeId)
        {
            Guid eventIdGuid = Guid.Empty;
            bool isIdValid = this.IsGuidValid(eventId, ref eventIdGuid);

            Guid attendeeIdGuid = Guid.Empty;
            bool isIdValid2 = this.IsGuidValid(attendeeId, ref attendeeIdGuid);

            if (!isIdValid && !isIdValid2)
            {
                return this.RedirectToAction(nameof(Index));
            }

            if (!await this.eventService.EventExistsByIdAsync(eventIdGuid))
            {
                return RedirectToAction(nameof(Edit), new { id = eventId });
            }

            var result = await eventService.RemoveAnttendeeAsync(eventIdGuid, attendeeIdGuid);

            return RedirectToAction(nameof(Edit), new { id = eventId });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(string? id)
        {
            Guid guidId = Guid.Empty;
            bool isIdValid = this.IsGuidValid(id, ref guidId);
            if (!isIdValid)
            {
                return this.RedirectToAction(nameof(Admin));
            }

            if (!await this.eventService.EventExistsByIdAsync(guidId))
            {
                return this.RedirectToAction(nameof(Admin));
            }

            DeleteEventViewModel? eventToDeleteViewModel =
                await this.eventService.GetEventForDeleteByIdAsync(guidId);

            if (eventToDeleteViewModel == null)
            {
                return this.RedirectToAction(nameof(Admin));
            }

            return this.View(eventToDeleteViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SoftDeleteConfirmed(DeleteEventViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            Guid eventId = Guid.Empty;
            bool isIdValid = this.IsGuidValid(viewModel.Id, ref eventId);
            if (!isIdValid)
            {
                return this.RedirectToAction(nameof(Admin));
            }

            bool isDeleted = await this.eventService
                .SoftDeleteEventAsync(eventId);
            if (!isDeleted)
            {
                TempData["ErrorMessage"] =
                    "Unexpected error occurred while trying to delete the event! Please contact system administrator!";
                return this.RedirectToAction(nameof(Delete), new { id = viewModel.Id });
            }

            return this.RedirectToAction(nameof(Admin));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Unjoin(string eventId)
        {
            Guid eventIdGuid = Guid.Empty;
            bool isIdValid = this.IsGuidValid(eventId, ref eventIdGuid);
            if (!isIdValid)
            {
                return this.RedirectToAction(nameof(Index));
            }

            if (!await this.eventService.EventExistsByIdAsync(eventIdGuid))
            {
                return this.RedirectToAction(nameof(Index));
            }

            Guid? userGuid = GetCurrectUserGuidId();

            if (!userGuid.HasValue)
            {
                return Unauthorized();
            }

            await this.eventService
                .UnjoinEventAsync(eventIdGuid, userGuid.Value);

            return this.RedirectToAction(nameof(Index));
        }
    }
}

