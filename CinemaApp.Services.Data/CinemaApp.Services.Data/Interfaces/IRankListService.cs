using System;
using CinemaApp.Web.ViewModels.RankList;

namespace CinemaApp.Services.Data.Interfaces
{
    using static CinemaApp.Common.EntityValidationConstants.RankList;
    public interface IRankListService
	{
		Task<RankListUserPaginationViewModel> GetAllUsersOrderedByRunnedDistanceAsync(Guid? userId, int pageNumber = 1, int pageSize = PageSizeConstant);

		Task DeletePassedEventsAndRunnedDistanceToTheParticipants();
    }
}

