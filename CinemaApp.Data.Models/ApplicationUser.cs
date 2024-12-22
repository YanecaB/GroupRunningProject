namespace CinemaApp.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            this.Id = Guid.NewGuid();
        }

        public string? Bio { get; set; }

        public bool IsBanned { get; set; } = false;

        public int RunnedDistance { get; set; } = 0;

        public virtual ICollection<Membership> Memberships { get; set; }
            = new HashSet<Membership>();

        public virtual ICollection<ApplicationUserEvent> ApplicationUserEvents { get; set; }
            = new HashSet<ApplicationUserEvent>();

        public virtual ICollection<Notification> Notifications { get; set; }
            = new HashSet<Notification>();

        public virtual ICollection<ApplicationUser> Friends { get; set; }
            = new HashSet<ApplicationUser>();
    }
}
