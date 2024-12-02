using CinemaApp.Data;
using Microsoft.AspNetCore.Mvc;
using CinemaApp.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CinemaApp.Web.Infrastructure.Extensions;
using CinemaApp.Web.ViewModels.Group;
using CinemaApp.Web.ViewModels.User;
using CinemaApp.Services.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaApp.Web.Controllers
{
    //[Authorize]
    public class GroupController : BaseController
    {
        private readonly IGroupService groupService;

        public GroupController(IGroupService groupService)            
        {
            this.groupService = groupService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var groups = await groupService.IndexGetAllAsync();
            
            return this.View(groups);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(GroupCreateViewModel model)
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

            await groupService.AddGroupAsync(model, userGuid.Value);

            return RedirectToAction(nameof(Admin));
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

            GroupDetailsViewModel? groupDetails = await this.groupService
                .GetGroupDetailsByIdAsync(guidId, userGuid.Value);

            return this.View(groupDetails);
        }

        [Authorize]
        public async Task<IActionResult> Follow(string? id)
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

            await this.groupService
                .FollowGroupAsync(guidId, userGuid.Value);

            return RedirectToAction(nameof(Index)); // TODO: Redirect to Following page
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveFollower(string groupId, string followerId)
        {
            Guid groupIdGuid = Guid.Empty;
            bool isIdValid = this.IsGuidValid(groupId, ref groupIdGuid);

            Guid followerIdGuid = Guid.Empty;
            bool isIdValid2 = this.IsGuidValid(followerId, ref followerIdGuid);

            if (!isIdValid && !isIdValid2)
            {
                return this.RedirectToAction(nameof(Index));
            }

            var result = await groupService.RemoveFollowerAsync(groupIdGuid, followerIdGuid);

            if (!result)
            {
                TempData["Error"] = "Failed to remove the follower. Please try again.";
            }
            else
            {
                TempData["Success"] = "Follower removed successfully.";
            }

            return RedirectToAction(nameof(Edit), new { id = groupId });
        }

        [Authorize]
        public async Task<IActionResult> Unfollow(string? id)
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

            await this.groupService.UnFollowGroupAsync(guidId, userGuid.Value);

            return RedirectToAction(nameof(Index)); // TODO: Redirect to Following page
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

            DeleteGroupViewModel? groupToDeleteViewModel =
                await this.groupService.GetGroupForDeleteByIdAsync(guidId);

            if (groupToDeleteViewModel == null)
            {
                return this.RedirectToAction(nameof(Admin));
            }

            return this.View(groupToDeleteViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SoftDeleteConfirmed(DeleteGroupViewModel group)
        {
            if (!ModelState.IsValid)
            {
                return this.View(group);
            }

            Guid guidId = Guid.Empty;
            bool isIdValid = this.IsGuidValid(group.Id, ref guidId);
            if (!isIdValid)
            {
                return this.RedirectToAction(nameof(Admin));
            }

            bool isDeleted = await this.groupService
                .SoftDeleteGroupAsync(guidId);
            if (!isDeleted)
            {
                TempData["ErrorMessage"] =
                    "Unexpected error occurred while trying to delete the group! Please contact system administrator!";
                return this.RedirectToAction(nameof(Delete), new { id = group.Id });
            }

            return this.RedirectToAction(nameof(Admin));
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

            IEnumerable<GroupIndexViewModel> groups =
               await this.groupService.GetAllAdminGroupsAsync(userGuid.Value);

            return this.View(groups);
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

            GroupEditViewModel? viewModel = await this.groupService
                .GetGroupForEditAsync(guidId);

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(GroupEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            bool isUpdated = await this.groupService
               .EditGroupAsync(viewModel);

            if (!isUpdated)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error occurred while updating the group! Please contact administrator");
                return this.View(viewModel);
            }

            return RedirectToAction(nameof(Admin));
        }
    }
}

