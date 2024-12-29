using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaApp.Services.Data;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.RankList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static CinemaApp.Common.EntityValidationConstants.RankList;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaApp.Web.Controllers
{
    [Authorize]
    public class RankListController : BaseController
    {
        private readonly IRankListService rankListService;

        public RankListController(IRankListService rankListService)
        {
            this.rankListService = rankListService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            Guid? userGuid = GetCurrectUserGuidId();
            if (!userGuid.HasValue)
            {
                return Unauthorized();
            }

            RankListUserPaginationViewModel usersInfo = await this.rankListService
            .GetAllUsersOrderedByRunnedDistanceAsync(userGuid, pageNumber);

            usersInfo.UserNumber += pageNumber * PageSizeConstant;

            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = usersInfo.TotalPages;
            ViewData["CurrentPage"] = pageNumber;

            return View(usersInfo);
        }
    }
}

