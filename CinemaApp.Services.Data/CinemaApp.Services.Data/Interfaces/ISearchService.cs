using System;
using CinemaApp.Web.ViewModels.Search;

namespace CinemaApp.Services.Data.Interfaces
{
	public interface ISearchService
	{
		Task<IEnumerable<SearchUserViewModel>> SearchUsersByNameAsync(string? username);
	}
}

