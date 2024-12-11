using System;
namespace CinemaApp.Web.ViewModels.RankList
{
	public class RankListUserPaginationViewModel
	{
		public ICollection<RankListUserViewModel> Users { get; set; } = null!;

		public RankListUserViewModel? First { get; set; }

        public RankListUserViewModel? Second { get; set; }

        public RankListUserViewModel? Third { get; set; }

        public int? PageNumber { get; set; } = 1;

        public int? PageSize { get; set; } = 2;

        public int? TotalPages { get; set; }

        public int? CurrentUserRank { get; set; }

        public RankListUserViewModel? CurrentUser { get; set; }
    }
}