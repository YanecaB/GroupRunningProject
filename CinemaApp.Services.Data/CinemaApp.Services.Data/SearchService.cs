using System;
using CinemaApp.Data.Models;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Search;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Services.Data
{
	public class SearchService : BaseService, ISearchService
	{
        private readonly UserManager<ApplicationUser> userManager;

        public SearchService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
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

