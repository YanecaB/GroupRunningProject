using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaApp.Services.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaApp.Web.Controllers
{
    public class RankListController : BaseController
    {
        private readonly IRankListService rankListService;

        public RankListController(IRankListService rankListService)
        {
            this.rankListService = rankListService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await this.rankListService
                .GetAllUsersOrderedByRunnedDistanceAsync();

            return View(users);
        }
    }
}

