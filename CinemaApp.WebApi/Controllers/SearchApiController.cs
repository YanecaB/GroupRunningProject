using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaApp.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SearchApiController : Controller
    {
        private readonly ISearchService searchService;

        public SearchApiController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        [HttpGet("")]
        public async Task<IActionResult> SearchUsers(string username)
        {
            var filteredUsers = await this.searchService.SearchUsersByNameAsync(username);

            return this.View(filteredUsers);
        }
    }
}
