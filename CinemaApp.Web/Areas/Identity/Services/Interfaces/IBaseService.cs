﻿namespace CinemaApp.Web.Areas.Identity.Services.Interfaces
{
    public interface IBaseService
    {
        bool IsGuidValid(string? id, ref Guid parsedGuid);
    }
}
