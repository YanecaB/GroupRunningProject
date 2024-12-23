﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CinemaApp.Data.Models;
using CinemaApp.Web.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Web.Areas.Identity.Controllers
{
    using static CinemaApp.Common.EntityValidationConstants.ApplicationUser;

    //[Authorize(Roles = UserRoleName)]
    [Authorize]
    [Area("Identity")]    
    public class AccountController : BaseController
    {
        //todo: move this to a service

        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        
        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            var user = await _userManager.Users
                .Include(u => u.ApplicationUserEvents)
                .FirstOrDefaultAsync(u => u.Id.ToString() == id);
            
            if (user == null && this.GetCurrectUserGuidId().ToString() != id)
            {
                return NotFound("User not found! :O");
            }

            var viewModel = new ApplicationUserDetailsViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Bio = string.IsNullOrEmpty(user.Bio) ? EmptyBioMessage : user.Bio,
                IsBanned = user.IsBanned,
                UserEvents = user.ApplicationUserEvents.ToList().Count(),
                Friends = user.Friends.Select(f => new ApplicationUserViewModel()
                {
                    Email = f.Email,
                    UserName = f.UserName,
                    Id = f.Id.ToString()
                }).ToList(),
                ProfilePicturePath = user.ProfilePicturePath
            };

            return this.View("Details", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await this._userManager.FindByIdAsync(this.GetCurrectUserGuidId().ToString());

            var viewModel = new ApplicationUserEditProfileViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Bio = string.IsNullOrEmpty(user.Bio) ? EmptyBioMessage : user.Bio,
                ProfilePicturePath = user.ProfilePicturePath
            };

            return this.View(viewModel);
        }

        //[HttpPost]
        //public async Task<IActionResult> Edit(ApplicationUserEditProfileViewModel viewModel)
        //{
        //    if ()
        //}
    }
}

