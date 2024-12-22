using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CinemaApp.Data.Models;
using CinemaApp.Web.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaApp.Web.Areas.Identity.Controllers
{
    using static Common.ApplicationConstants;

    //[Authorize(Roles = UserRoleName)]
    [Area("Identity")]    
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null && this.GetCurrectUserGuidId().ToString().ToLower() != id.ToLower())
            {
                return NotFound("User not found! :O");
            }

            var viewModel = new ApplicationUserDetailsViewModel
            {
                Id = user.Id,
                Email = user.Email,
                IsBanned = user.IsBanned,
                Memberships = user.Memberships.Select(m => m.Group.Name).ToList(),
                UserEvents = user.ApplicationUserEvents.Select(e => e.Event.Title).ToList(),
                Notifications = user.Notifications.Select(n => n.Message).ToList()
            };

            return this.View(viewModel);
        }
    }
}

