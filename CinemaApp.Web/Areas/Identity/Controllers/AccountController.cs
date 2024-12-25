using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CinemaApp.Data.Models;
using CinemaApp.Web.Areas.Identity.Services.Interfaces;
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

        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            Guid guidId = Guid.Empty;
            bool isIdValid = this.IsGuidValid(id, ref guidId);
            if (!isIdValid)
            {
                return NotFound();
            }

            var infoAboutTheUser = await this.accountService.GetDetailsByIdAsync(guidId, this.GetCurrectUserGuidId());

            return this.View(infoAboutTheUser);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            Guid id = this.GetCurrectUserGuidId();
            var viewModelForEdit = await this.accountService.GetInfoForEditProfileByIdAsync(id);

            return this.View(viewModelForEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUserEditProfileViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var editProfile = await this.accountService.EditProfileByIdAsync(viewModel);

            if (editProfile)
            {
                return RedirectToAction(nameof(Details), new { id = viewModel.Id });
            }
            else
            {
                return View(viewModel);
            }
        }

        /*

        user.Email = Input.Email;

                await _userStore.SetUserNameAsync(user, Input.Username, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

         */
    }
}

