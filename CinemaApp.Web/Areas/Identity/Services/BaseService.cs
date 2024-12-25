using System;
using CinemaApp.Web.Areas.Identity.Services.Interfaces;

namespace CinemaApp.Web.Areas.Identity.Services
{
	public class BaseService : IBaseService
	{
        public bool IsGuidValid(string? id, ref Guid parsedGuid)
        {            
            if (String.IsNullOrWhiteSpace(id))
            {
                return false;
            }
            
            bool isGuidValid = Guid.TryParse(id, out parsedGuid);
            if (!isGuidValid)
            {
                return false;
            }

            return true;
        }
    }
}

