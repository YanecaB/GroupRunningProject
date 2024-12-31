using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.FriendRequest;
using CinemaApp.Web.ViewModels.Search;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaApp.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendRequestApiController : ControllerBase
    {
        private readonly IFriendRequestService friendRequestService;

        public FriendRequestApiController(IFriendRequestService friendRequestService)
        {
            this.friendRequestService = friendRequestService;
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(SearchUserViewModel), StatusCodes.Status200OK)]        
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmRequest([FromBody] ReplyOnFriendRequestViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            var confirmRequest = await this.friendRequestService.ConfirmFriendRequestAsync(viewModel);
            
            //TODO: SEND NOTIFICATION TO THE SENDER
            return confirmRequest ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(SearchUserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRequest([FromBody] ReplyOnFriendRequestViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            var deleteRequest = await this.friendRequestService.DeleteFriendRequestAsync(viewModel);

            return deleteRequest ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}

