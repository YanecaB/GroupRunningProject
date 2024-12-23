﻿namespace CinemaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using CinemaApp.Web.Infrastructure.Extensions;    

    public class BaseController : Controller
    {
        protected bool IsGuidValid(string? id, ref Guid parsedGuid)
        {
            // Non-existing parameter in the URL
            if (String.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            // Invalid parameter in the URL
            bool isGuidValid = Guid.TryParse(id, out parsedGuid);
            if (!isGuidValid)
            {
                return false;
            }

            return true;
        }

        protected Guid GetCurrectUserGuidId()
        {
            // This will throw an exception if User.GetUserIdAsGuid() returns null.
            return User.GetUserIdAsGuid() ?? throw new InvalidOperationException("User ID is not available.");
        }
    }
}
