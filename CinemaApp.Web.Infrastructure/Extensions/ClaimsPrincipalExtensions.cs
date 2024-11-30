
using System;
using System.Security.Claims;

namespace CinemaApp.Web.Infrastructure.Extensions
{
	public static class ClaimsPrincipalExtensions
	{
        public static Guid? GetUserIdAsGuid(this ClaimsPrincipal? userClaimsPrincipal)
        {
            string? userId = userClaimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userId, out Guid userGuid))
            {
                return userGuid;
            }

            return null;
        }
    }
}

