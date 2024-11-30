using CinemaApp.Data;
using Microsoft.AspNetCore.Mvc;
using CinemaApp.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CinemaApp.Web.Infrastructure.Extensions;
using CinemaApp.Web.ViewModels.Group;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaApp.Web.Controllers
{
    public class GroupController : BaseController
    {
        private readonly IGroupService cinemaService;

        public GroupController(IGroupService cinemaService)            
        {
            this.cinemaService = cinemaService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var groups = await cinemaService.IndexGetAllAsync();
            
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
            
            Guid? userGuid = User.GetUserIdAsGuid();
            if (userGuid == null)
            {
                return Unauthorized();
            }
           
            await cinemaService.AddGroupAsync(model, userGuid.Value);

            return RedirectToAction(nameof(Index));
        }
    }
}

