using System;
using Microsoft.Extensions.DependencyInjection;
using static CinemaApp.Common.EntityValidationMessages;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using CinemaApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using CinemaApp.Data.Seeding.DTOs;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

using static CinemaApp.Common.EntityValidationConstants.Group;
using Group = CinemaApp.Data.Models.Group;
using Event = CinemaApp.Data.Models.Event;
using ApplicationUser = CinemaApp.Data.Models.ApplicationUser;

namespace CinemaApp.Data.Seeding
{
	public class DbSeeder
	{
        public static async Task SeedUsersAsync(IServiceProvider services, string jsonPath)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var logger = services.GetRequiredService<ILogger<DbSeeder>>();

            try
            {
                var json = await File.ReadAllTextAsync(jsonPath);
                var userDtos = JsonSerializer.Deserialize<List<ImportUserDto>>(json);

                if (userDtos == null || !userDtos.Any())
                {
                    logger.LogWarning("No users found in the provided JSON file.");
                    return;
                }

                foreach (var userDto in userDtos)
                {
                    if (!IsValid(userDto))
                    {
                        continue;
                    }

                    if (await userManager.FindByEmailAsync(userDto.Email) != null)
                    {
                        logger.LogInformation($"User with email {userDto.Email} already exists.");
                        continue;
                    }

                    var user = new ApplicationUser
                    {
                        Id = userDto.Id,
                        Email = userDto.Email,
                        UserName = userDto.Username,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, userDto.Password);

                    if (result.Succeeded)
                    {
                        logger.LogInformation($"User {userDto.Username} created successfully.");
                    }
                    else
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        logger.LogError($"Failed to create user {userDto.Username}: {errors}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while seeding users: {ex.Message}");
            }
        }

        public static async Task SeedGroupsAsync(IServiceProvider services, string jsonPath)
        {
            await using CinemaDbContext dbContext = services
                .GetRequiredService<CinemaDbContext>();

            ICollection<Group> allGroups = await dbContext
               .Groups
               .ToArrayAsync();

            try
            {
                var json = await File.ReadAllTextAsync(jsonPath);
                var groupDtos = JsonSerializer.Deserialize<List<ImportGroupDto>>(json);


                foreach (var groupDto in groupDtos)
                {
                    if (!IsValid(groupDto))
                    {
                        continue;
                    }

                    Guid groupGuid = Guid.Empty;
                    if (!IsGuidValid(groupDto.Id, ref groupGuid))
                    {
                        continue;
                    }

                    Guid adminGuid = Guid.Empty;
                    if (!IsGuidValid(groupDto.AdminId, ref adminGuid))
                    {
                        continue;
                    }

                    bool isCreatedDateValid = DateTime
                        .TryParse(groupDto.CreatedDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime createdDate);
                    if (!isCreatedDateValid)
                    {
                        continue;
                    }

                    if (allGroups.Any(g => g.Id.ToString().ToLower() == groupDto.Id.ToLower(CultureInfo.CurrentCulture)))
                    {                        
                        continue;
                    }

                    var group = new Group
                    {
                        Id = groupGuid,
                        Description = groupDto.Description,
                        Location = groupDto.Location,
                        CreatedDate = createdDate,
                        Name = groupDto.Name,
                        AdminId = adminGuid
                    };

                    await dbContext.Groups.AddAsync(group);
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while seeding the groups in the database!");
            }
        }

        public static async Task SeedMembershipsAsync(IServiceProvider services, string jsonPath)
        {
            await using CinemaDbContext dbContext = services
                .GetRequiredService<CinemaDbContext>();

            ICollection<Membership> allEvents = await dbContext
               .Memberships
               .ToArrayAsync();

            try
            {
                var json = await File.ReadAllTextAsync(jsonPath);
                var membershipDtos = JsonSerializer.Deserialize<List<ImportMembershipDto>>(json);


                foreach (var membershipDto in membershipDtos)
                {
                    if (!IsValid(membershipDto))
                    {

                        continue;
                    }

                    Guid membershipGuid = Guid.Empty;
                    if (!IsGuidValid(membershipDto.Id, ref membershipGuid))
                    {
                        continue;
                    }

                    Guid userGuid = Guid.Empty;
                    if (!IsGuidValid(membershipDto.ApplicationUserId, ref userGuid))
                    {
                        continue;
                    }

                    Guid groupGuid = Guid.Empty;
                    if (!IsGuidValid(membershipDto.GroupId, ref groupGuid))
                    {
                        continue;
                    }
                    
                    if (allEvents.Any(e => e.Id.ToString().ToLower() == membershipDto.Id.ToLower(CultureInfo.CurrentCulture)))
                    {
                        continue;
                    }

                    var membership = new Membership
                    {
                        Id = membershipGuid,
                        ApplicationUserId = userGuid,
                        GroupId = groupGuid,
                        JoinDate = membershipDto.JoinDate
                    };

                    await dbContext.Memberships.AddAsync(membership);
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while seeding the memberships in the database!");
            }
        }        

        public static async Task SeedEventsAsync(IServiceProvider services, string jsonPath)
        {
            await using CinemaDbContext dbContext = services
                .GetRequiredService<CinemaDbContext>();

            ICollection<Event> allEvents = await dbContext
               .Events
               .ToArrayAsync();

            try
            {
                var json = await File.ReadAllTextAsync(jsonPath);
                var eventDtos = JsonSerializer.Deserialize<List<ImportEventDto>>(json);


                foreach (var eventDto in eventDtos)
                {
                    if (!IsValid(eventDto))
                    {
                        continue;
                    }

                    Guid eventGuid = Guid.Empty;
                    if (!IsGuidValid(eventDto.Id, ref eventGuid))
                    {
                        continue;
                    }

                    Guid organizerGuid = Guid.Empty;
                    if (!IsGuidValid(eventDto.OrganizerId, ref organizerGuid))
                    {
                        continue;
                    }

                    Guid groupGuid = Guid.Empty;
                    if (!IsGuidValid(eventDto.GroupId, ref groupGuid))
                    {
                        continue;
                    }

                    bool isCreatedDateValid = DateTime
                        .TryParse(eventDto.Date, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
                    if (!isCreatedDateValid)
                    {
                        continue;
                    }

                    if (allEvents.Any(e => e.Id.ToString().ToLower() == eventDto.Id.ToLower(CultureInfo.CurrentCulture)))
                    {
                        continue;
                    }

                    var eventEntity = new Event
                    {
                        Id = eventGuid,
                        Description = eventDto.Description,
                        Location = eventDto.Location,
                        Date = date,
                        Title = eventDto.Title,
                        OrganizerId = organizerGuid,
                        GroupId = groupGuid,
                        Distance = eventDto.Distance
                    };

                    await dbContext.Events.AddAsync(eventEntity);
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {                
                Console.WriteLine("Error occurred while seeding the events in the database!");
            }
        }

        private static bool IsValid(object obj)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            var context = new ValidationContext(obj);
            var isValid = Validator.TryValidateObject(obj, context, validationResults);

            return isValid;
        }

        private static bool IsGuidValid(string id, ref Guid parsedGuid)
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
    }
}

