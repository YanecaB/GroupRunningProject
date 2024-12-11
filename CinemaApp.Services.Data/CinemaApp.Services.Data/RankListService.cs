using System;
using CinemaApp.Common;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Event;
using CinemaApp.Web.ViewModels.RankList;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Services.Data
{
    using static CinemaApp.Common.EntityValidationConstants.RankList;

    public class RankListService : BaseService, IRankListService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<Event, Guid> eventRepository;
        private readonly IRepository<ApplicationUserEvent, object> userEventRepository;

        public RankListService(UserManager<ApplicationUser> userManager,
            IRepository<Event, Guid> eventRepository,
            IRepository<ApplicationUserEvent, object> userEventRepository)
        {
            this.userManager = userManager;
            this.eventRepository = eventRepository;
            this.userEventRepository = userEventRepository;
        }

        public async Task<RankListUserPaginationViewModel> GetAllUsersOrderedByRunnedDistanceAsync(Guid? userId, int pageNumber = 1, int pageSize = PageSizeConstant)
        {
            RankListUserPaginationViewModel rankListAllInfo = new RankListUserPaginationViewModel();

            List<RankListUserViewModel> users = await this.userManager.Users
                .Where(u => u.IsBanned == false)
                .Select(u => new RankListUserViewModel()
                {
                    Id = u.Id.ToString(),
                    UserName = u.UserName,
                    RunnedDistance = u.RunnedDistance
                })
                .OrderByDescending(u => u.RunnedDistance)
                .ToListAsync();

            int totalEvent = users.Count() - 3;
            int totalPages = (int)Math.Ceiling(totalEvent / (double)pageSize);

            rankListAllInfo.First = users[0];
            rankListAllInfo.Second = users[1];
            rankListAllInfo.Third = users[2];
            
            var currentUser = users.FirstOrDefault(u => u.Id.ToString().ToLower() == userId.ToString().ToLower());
            if (currentUser != null)
            {
                rankListAllInfo.CurrentUserRank = users.FindIndex(u => u.Id == currentUser.Id) + 1;
                rankListAllInfo.CurrentUser = currentUser;
            }

            users = users
                .Skip(((pageNumber - 1) * pageSize) + 3)
                .Take(pageSize)
                .ToList();

            rankListAllInfo.Users = users;
            rankListAllInfo.PageNumber = pageNumber;
            rankListAllInfo.PageSize = pageNumber;
            rankListAllInfo.TotalPages = totalPages;

            return rankListAllInfo;
        }

        public async Task DeletePassedEventsAndRunnedDistanceToTheParticipants()
        {
            var passedEvents = await this.eventRepository
                .GetAllAttached()
                .Include(e => e.UsersEvents)
                .ThenInclude(ue => ue.ApplicationUser)
                .Where(e => e.Date < DateTime.Now && e.IsDeleted == false && e.IsPassed == false)
                .ToListAsync();

            foreach (var passedEvent in passedEvents)
            {
                foreach (var userEvent in passedEvent.UsersEvents)
                {
                    if (userEvent.ApplicationUser.IsBanned == false)
                    {
                        userEvent.ApplicationUser.RunnedDistance += passedEvent.Distance;
                        await this.userManager.UpdateAsync(userEvent.ApplicationUser);
                    }
                }

                passedEvent.IsPassed = true;
                await this.eventRepository.UpdateAsync(passedEvent);
            }
        }
    }
}

