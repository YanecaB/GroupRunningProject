﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaApp.Services.Data;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Event;
using CinemaApp.Web.ViewModels.Group;
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
    }
}

