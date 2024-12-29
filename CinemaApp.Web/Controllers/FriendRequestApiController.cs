using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.FriendRequest;
using CinemaApp.Web.ViewModels.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendRequestApiController : BaseController
    {
        private readonly IFriendRequestService friendRequestService;

        public FriendRequestApiController(IFriendRequestService friendRequestService)
        {
            this.friendRequestService = friendRequestService;
        }

        [HttpPost("SendFriendRequest")]
        [ProducesResponseType(typeof(SearchUserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize]
        public async Task<IActionResult> SendFriendRequest([FromForm] string? username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return this.BadRequest("Username cannot be empty.");
            }

            Guid currentUserId = GetCurrectUserGuidId();

            bool isSent = await this.friendRequestService.SendFriendRequestAsync(username, currentUserId);

            if (isSent)
            {
                return this.Ok();
            }
            else
            {
                return this.StatusCode(StatusCodes.Status404NotFound);
            }
        }

        [HttpGet("Test")]
        public IActionResult Test()
        {
            Guid currentUserId = GetCurrectUserGuidId();

            return Ok("Controller is working");
        }
    }
}

