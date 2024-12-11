using System;
namespace CinemaApp.Web.ViewModels.RankList
{
    using static CinemaApp.Common.EntityValidationConstants.RankList;

    public class RankListUserPaginationViewModel
	{
		public ICollection<RankListUserViewModel> Users { get; set; } = null!;

		public RankListUserViewModel? First { get; set; }

        public RankListUserViewModel? Second { get; set; }

        public RankListUserViewModel? Third { get; set; }
        
        public int? PageNumber { get; set; } = 1;

        public int? PageSize { get; set; } = PageSizeConstant;

        public int? UserNumber { get; set; } = 4 - PageSizeConstant;

        public int? TotalPages { get; set; }

        public int? CurrentUserRank { get; set; }

        public RankListUserViewModel? CurrentUser { get; set; }
    }
}