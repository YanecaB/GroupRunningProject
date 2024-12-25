using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaApp.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaApp.Web.Controllers
{
    [Authorize]
    public class SearchController : BaseController
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        //
        [HttpGet]
        public async Task<IActionResult> Index(string username)
        {
            var filteredUsers = await this.searchService.SearchUsersByNameAsync(username);

            return this.View(filteredUsers);
        }
    }
}

