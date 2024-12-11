using System;
using CinemaApp.Web.ViewModels.RankList;

namespace CinemaApp.Services.Data.Interfaces
{
	public interface IRankListService
	{
		Task<ICollection<RankListUserViewModel>> GetAllUsersOrderedByRunnedDistanceAsync();

		Task DeletePassedEventsAndRunnedDistanceToTheParticipants();
    }
}

