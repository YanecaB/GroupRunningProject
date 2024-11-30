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
            optionsBuilder.UseSqlServer("Server=localhost;Database=GroupRunning;User ID=sa;Password=awesome1&;Pooling=false;Encrypt=False;");
        }

        //public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;

        public virtual DbSet<Group> Groups { get; set; } = null!;

        public virtual DbSet<Event> Events { get; set; } = null!;
                        
        public virtual DbSet<Membership> Memberships { get; set; } = null!;

        public virtual DbSet<ApplicationUserEvent> UsersEvents { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationship between Event and ApplicationUser (Organizer)
            modelBuilder.Entity<Event>()
                .HasOne(e => e.ApplicationUser)
                .WithMany()
                .HasForeignKey(e => e.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            // Configure relationship between Event and Group
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Group)
                .WithMany(g => g.Events) // Ensure the Group model has a collection of Events
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            // Configure many-to-many relationship for UsersGroups
            modelBuilder.Entity<Membership>()
                .HasKey(ug => new { ug.ApplicationUserId, ug.GroupId });

            modelBuilder.Entity<Membership>()
                .HasOne(ug => ug.ApplicationUser)
                .WithMany(u => u.Memberships)
                .HasForeignKey(ug => ug.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Membership>()
                .HasOne(ug => ug.Group)
                .WithMany(g => g.Memberships)
                .HasForeignKey(ug => ug.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure many-to-many relationship for UsersEvents
            modelBuilder.Entity<ApplicationUserEvent>()
                .HasKey(ue => new { ue.ApplicationUserId, ue.EventId });

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
