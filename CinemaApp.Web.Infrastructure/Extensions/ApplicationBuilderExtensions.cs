﻿namespace CinemaApp.Web.Infrastructure.Extensions
{
    using CinemaApp.Data.Models;
    using CinemaApp.Data.Seeding;
    using Data;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using static Common.ApplicationConstants;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();
            
            CinemaDbContext dbContext = serviceScope
                .ServiceProvider
                .GetRequiredService<CinemaDbContext>()!;
            dbContext.Database.Migrate();

            return app;
        }

        public static IApplicationBuilder SeedAdministrator(this IApplicationBuilder app, string email, string username, string password)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            RoleManager<IdentityRole<Guid>>? roleManager = serviceProvider
                .GetService<RoleManager<IdentityRole<Guid>>>();
            IUserStore<ApplicationUser>? userStore = serviceProvider
                .GetService<IUserStore<ApplicationUser>>();
            UserManager<ApplicationUser>? userManager = serviceProvider
                .GetService<UserManager<ApplicationUser>>();

            if (roleManager == null)
            {
                throw new ArgumentNullException(nameof(roleManager),
                    $"Service for {typeof(RoleManager<IdentityRole<Guid>>)} cannot be obtained!");
            }

            if (userStore == null)
            {
                throw new ArgumentNullException(nameof(userStore),
                    $"Service for {typeof(IUserStore<ApplicationUser>)} cannot be obtained!");
            }

            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager),
                    $"Service for {typeof(UserManager<ApplicationUser>)} cannot be obtained!");
            }

            Task.Run(async () =>
            {
                bool roleExists = await roleManager.RoleExistsAsync(AdminRoleName);
                IdentityRole<Guid>? adminRole = null;
                if (!roleExists)
                {
                    adminRole = new IdentityRole<Guid>(AdminRoleName);

                    IdentityResult result = await roleManager.CreateAsync(adminRole);
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException($"Error occurred while creating the {AdminRoleName} role!");
                    }
                }
                else
                {
                    adminRole = await roleManager.FindByNameAsync(AdminRoleName);
                }

                bool roleExists2 = await roleManager.RoleExistsAsync(UserRoleName);                 
                if (!roleExists2)
                {
                    IdentityRole<Guid>? userRole = new IdentityRole<Guid>(UserRoleName);

                    IdentityResult result = await roleManager.CreateAsync(userRole);
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException($"Error occurred while creating the {UserRoleName} role!");
                    }
                }

                ApplicationUser? adminUser = await userManager.FindByEmailAsync(email);
                if (adminUser == null)
                {
                    adminUser = await
                        CreateAdminUserAsync(email, username, password, userStore, userManager);
                }

                if (await userManager.IsInRoleAsync(adminUser, AdminRoleName))
                {
                    return app;
                }

                IdentityResult userResult = await userManager.AddToRoleAsync(adminUser, AdminRoleName);
                if (!userResult.Succeeded)
                {
                    throw new InvalidOperationException($"Error occurred while adding the user {username} to the {AdminRoleName} role!");
                }

                return app;
            })
                .GetAwaiter()
                .GetResult();

            return app;
        }

        private static async Task<ApplicationUser> CreateAdminUserAsync(string email, string username, string password,
            IUserStore<ApplicationUser> userStore, UserManager<ApplicationUser> userManager)
        {
            ApplicationUser applicationUser = new ApplicationUser
            {
                Email = email
            };

            await userStore.SetUserNameAsync(applicationUser, username, CancellationToken.None);
            IdentityResult result = await userManager.CreateAsync(applicationUser, password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error occurred while registering {AdminRoleName} user!");
            }

            return applicationUser;
        }

        public static IApplicationBuilder SeedUsers(this IApplicationBuilder app, string jsonPath)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            Task.Run(async () =>
            {
                await DbSeeder.SeedUsersAsync(serviceProvider, jsonPath);
            })
                .GetAwaiter()
                .GetResult();

            return app;
        }

        public static IApplicationBuilder SeedGroups(this IApplicationBuilder app, string jsonPath)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            Task.Run(async () =>
            {
                await DbSeeder.SeedGroupsAsync(serviceProvider, jsonPath);
            })
                .GetAwaiter()
                .GetResult();

            return app;
        }

        public static IApplicationBuilder SeedMemberships(this IApplicationBuilder app, string jsonPath)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            Task.Run(async () =>
            {
                await DbSeeder.SeedMembershipsAsync(serviceProvider, jsonPath);
            })
                .GetAwaiter()
                .GetResult();

            return app;
        }

        public static IApplicationBuilder SeedEvents(this IApplicationBuilder app, string jsonPath)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            Task.Run(async () =>
            {
                await DbSeeder.SeedEventsAsync(serviceProvider, jsonPath);
            })
                .GetAwaiter()
                .GetResult();

            return app;
        }
    }
}
