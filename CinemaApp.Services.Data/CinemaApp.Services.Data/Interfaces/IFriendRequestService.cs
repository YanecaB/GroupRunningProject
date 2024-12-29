using System;
namespace CinemaApp.Services.Data.Interfaces
{
	public interface IFriendRequestService
	{
        Task<bool> SendFriendRequestAsync(string? username, Guid senderId);
    }
}

