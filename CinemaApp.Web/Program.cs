namespace CinemaApp.Web
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    using Infrastructure.Extensions;
    using CinemaApp.Services.Data;
    using CinemaApp.Services.Data.Interfaces;
    using static Common.ApplicationConstants;
    using CinemaApp.Web.Infrastructure.BackgroundTasks;
    using Microsoft.AspNetCore.Mvc;
    using CinemaApp.Web.Areas.Identity.Services.Interfaces;
    using Microsoft.AspNetCore.Http.Features;
    using CinemaApp.Web.Controllers;

    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetConnectionString("Server=localhost;Database=GroupRunning;User ID=sa;Password=awesome1&;Pooling=false;Encrypt=False;")!;
            string adminEmail = builder.Configuration.GetValue<string>("Administrator:Email")!;
            string adminUsername = builder.Configuration.GetValue<string>("Administrator:Username")!;
            string adminPassword = builder.Configuration.GetValue<string>("Administrator:Password")!;
            string usersJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                builder.Configuration.GetValue<string>("Seed:UsersJson")!);
            string groupsJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                builder.Configuration.GetValue<string>("Seed:GroupsJson")!);
            string membershipsJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                builder.Configuration.GetValue<string>("Seed:MembershipsJson")!);
            string eventsJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                builder.Configuration.GetValue<string>("Seed:EventsJson")!);
            
            // Add services to the container.
            builder.Services
                .AddDbContext<CinemaDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });

            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole<Guid>>(cfg =>
                {
                    ConfigureIdentity(builder, cfg);
                })
                .AddEntityFrameworkStores<CinemaDbContext>()
                .AddRoles<IdentityRole<Guid>>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddUserManager<UserManager<ApplicationUser>>();

            builder.Services.ConfigureApplicationCookie(cfg =>
            {
                cfg.LoginPath = "/Identity/Account/Login";
            });
            
            builder.Services.RegisterRepositories(typeof(ApplicationUser).Assembly);
            builder.Services.RegisterUserDefinedServices(typeof(IGroupService).Assembly);
            builder.Services.RegisterUserDefinedServices(typeof(CinemaApp.Web.Areas.Identity.Services.Interfaces.IBaseService).Assembly);

            //builder.Services.RegisterUserDefinedServices(typeof(IAccountService).Assembly);

            builder.Services.AddControllersWithViews(cfg =>
            {
                cfg.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            builder.Services.AddControllers()
                .AddApplicationPart(typeof(FriendRequestApiController).Assembly);

            builder.Services.AddRazorPages();

            // Register the background task as a hosted service
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddHostedService<NotificationScheduler>();
            builder.Services.AddHostedService<RemovePassedEventsAndAddRunnedDistance>();

            //uploading the files !!!
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 10485760; // 10MB
            });

            builder.Services.AddSwaggerGen();
            //// Register the notification service
            //builder.Services.AddScoped<INotificationService, NotificationService>();
            WebApplication app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API v1");
                });
            }
            

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //builder.Services.AddCors(cfg =>
            //{
            //    cfg.AddPolicy("AllowAll", policyBld =>
            //    {
            //        policyBld.AllowAnyHeader()
            //                  .AllowAnyMethod()
            //                  .AllowAnyOrigin();
            //    });
            //});

            app.UseCors("AllowMyServer");//app.UseCors("AllowAll");

            //if (!string.IsNullOrWhiteSpace(webAppOrigin))
            //{
            //    app.UseCors("AllowMyServer");
            //}
            //else
            //{
            //    app.UseCors("AllowAll");
            //}

            // Authorization can work only if we know who uses the application -> We need Authentication
            app.UseAuthentication(); // First -> Who am I?

            app.Use((context, next) =>
            {
                if (context.User.Identity?.IsAuthenticated == true && context.Request.Path == "/")
                {
                    if (context.User.IsInRole(AdminRoleName))
                    {
                        context.Response.Redirect("/Admin/Home/Index");
                        return Task.CompletedTask;
                    }
                }

                return next();
            });

            app.UseAuthorization(); // Second -> What can I do?

            app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");

            if (app.Environment.IsDevelopment())
            {
                app.SeedAdministrator(adminEmail, adminUsername, adminPassword);
                app.SeedUsers(usersJsonPath);
                app.SeedGroups(groupsJsonPath);
                app.SeedMemberships(membershipsJsonPath);
                app.SeedEvents(eventsJsonPath);
            }

            app.MapControllers();

            app.MapControllerRoute(
               name: "Areas",
               pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "Errors",
                pattern: "{controller=Home}/{action=Index}/{statusCode?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages(); // Add routing to Identity Razor Pages

            app.ApplyMigrations();

            app.Run();
        }

        private static void ConfigureIdentity(WebApplicationBuilder builder, IdentityOptions cfg)
        {
            cfg.Password.RequireDigit =
                builder.Configuration.GetValue<bool>("Identity:Password:RequireDigits");
            cfg.Password.RequireLowercase =
                builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
            cfg.Password.RequireUppercase =
                builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
            cfg.Password.RequireNonAlphanumeric =
                builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumerical");
            cfg.Password.RequiredLength =
                builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
            cfg.Password.RequiredUniqueChars =
                builder.Configuration.GetValue<int>("Identity:Password:RequiredUniqueCharacters");

            cfg.SignIn.RequireConfirmedAccount =
                builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
            cfg.SignIn.RequireConfirmedEmail =
                builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedEmail");
            cfg.SignIn.RequireConfirmedPhoneNumber =
                builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedPhoneNumber");

            cfg.User.RequireUniqueEmail =
                builder.Configuration.GetValue<bool>("Identity:User:RequireUniqueEmail");
        }
    }
}