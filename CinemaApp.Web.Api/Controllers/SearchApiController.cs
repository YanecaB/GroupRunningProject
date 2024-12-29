using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.Controllers;
using CinemaApp.Web.ViewModels.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaApp.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchApiController : BaseController
    {
        private readonly ISearchService searchService;

        public SearchApiController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        //[HttpGet("[action]/{username?}")]
        [HttpGet("SearchUsers")]
        [ProducesResponseType(typeof(SearchUserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
        public async Task<IActionResult> SearchUsers(string? username)
        {
            try
            {
                var filteredUsers = await this.searchService.SearchUsersByNameAsync(username);
                
                return this.Ok(filteredUsers);                               
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("SendFriendRequest")]
        [ProducesResponseType(typeof(SearchUserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize]
        public async Task<IActionResult> SendFriendRequest([FromQuery] string? username)
        {
            Guid currentUserId = GetCurrectUserGuidId();

            bool isSent = await this.searchService.SendFriendRequestAsync(username, currentUserId);

            if (isSent)
            {
                return this.Ok();
            }
            else
            {
                return this.StatusCode(StatusCodes.Status404NotFound);
            }
        }
    }
}

