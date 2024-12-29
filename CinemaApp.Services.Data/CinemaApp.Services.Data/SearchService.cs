using System;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Search;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Services.Data
{
	public class SearchService : BaseService, ISearchService
	{
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<FriendRequest, Guid> friendRequestRepository;

        public SearchService(UserManager<ApplicationUser> userManager,
            IRepository<FriendRequest, Guid> friendRequestRepository)
        {
            this.userManager = userManager;
            this.friendRequestRepository = friendRequestRepository;
        }

        public async Task<IEnumerable<SearchUserViewModel>> SearchUsersByNameAsync(string? username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return Enumerable.Empty<SearchUserViewModel>();
            }

            return await userManager.Users
                .Where(u => u.UserName.ToLower().StartsWith(username.ToLower()))
                .Select(u => new SearchUserViewModel()
                {
                    Username = u.UserName,
                    ProfilePicturePath = u.ProfilePicturePath
                })
                .ToArrayAsync();
        }        
    }
}

