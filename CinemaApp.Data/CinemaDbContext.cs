namespace CinemaApp.Data
{
    using System.Reflection;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class CinemaDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public CinemaDbContext()
        {
            
        }

        public CinemaDbContext(DbContextOptions options)
            : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=CinemaApp2;User ID=sa;Password=awesome1&;Pooling=false;Encrypt=False;");
        }

        public virtual DbSet<Movie> Movies { get; set; } = null!;

        public virtual DbSet<Cinema> Cinemas { get; set; } = null!;

        public virtual DbSet<CinemaMovie> CinemasMovies { get; set; } = null!;

        public virtual DbSet<Group> Groups { get; set; } = null!;

        public virtual DbSet<Event> Events { get; set; } = null!;

        public virtual DbSet<ApplicationUserGroup> UsersGroups { get; set; } = null!;

        public virtual DbSet<ApplicationUserEvent> UsersEvents { get; set; } = null!;

        public virtual DbSet<ApplicationUserMovie> UsersMovies { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationship for UsersGroups
            modelBuilder.Entity<ApplicationUserGroup>()
                .HasKey(ug => new { ug.ApplicationUserId, ug.GroupId }); // Composite key

            modelBuilder.Entity<ApplicationUserGroup>()
                .HasOne(ug => ug.ApplicationUser)
                .WithMany(u => u.ApplicationUserGroups)
                .HasForeignKey(ug => ug.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUserGroup>()
                .HasOne(ug => ug.Group)
                .WithMany(g => g.UsersGroups)
                .HasForeignKey(ug => ug.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure many-to-many relationship for UsersEvents
            modelBuilder.Entity<ApplicationUserEvent>()
                .HasKey(ue => new { ue.ApplicationUserId, ue.EventId }); // Composite key

            modelBuilder.Entity<ApplicationUserEvent>()
                .HasOne(ue => ue.ApplicationUser)
                .WithMany(u => u.ApplicationUserEvents)
                .HasForeignKey(ue => ue.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUserEvent>()
                .HasOne(ue => ue.Event)
                .WithMany(e => e.UsersEvents)
                .HasForeignKey(ue => ue.EventId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
