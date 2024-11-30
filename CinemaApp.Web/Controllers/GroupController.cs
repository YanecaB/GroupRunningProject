using CinemaApp.Data;
using Microsoft.AspNetCore.Mvc;
using CinemaApp.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CinemaApp.Web.Infrastructure.Extensions;
using CinemaApp.Web.ViewModels.Group;

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
        public async Task<IActionResult> Details(string id)
        {
            Guid guidId = Guid.Empty;
            bool isIdValid = this.IsGuidValid(id, ref guidId);
            if (!isIdValid)
            {
                return this.RedirectToAction(nameof(Index));
            }

            GroupDetailsViewModel groupDetails = await this.groupService
                .GetGroupDetailsByIdAsync(guidId);

            return this.View(groupDetails);
        }

        [Authorize]
        public async Task<IActionResult> Follow(string id)
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
    }
}

