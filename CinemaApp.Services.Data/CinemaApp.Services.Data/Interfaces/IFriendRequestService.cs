using System;
using CinemaApp.Web.ViewModels.FriendRequest;

namespace CinemaApp.Services.Data.Interfaces
{
	public interface IFriendRequestService
	{
        Task<bool> SendFriendRequestAsync(string? username, Guid senderId);

        Task<bool> ConfirmFriendRequestAsync(ReplyOnFriendRequestViewModel viewModel);

        Task<bool> DeleteFriendRequestAsync(ReplyOnFriendRequestViewModel viewModel);
    }
}

