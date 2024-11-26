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

        public virtual ICollection<ApplicationUserMovie> ApplicationUserMovies { get; set; }
            = new HashSet<ApplicationUserMovie>();

        public virtual ICollection<ApplicationUserGroup> ApplicationUserGroups { get; set; }
            = new HashSet<ApplicationUserGroup>();

        public virtual ICollection<ApplicationUserEvent> ApplicationUserEvents { get; set; }
            = new HashSet<ApplicationUserEvent>();
    }
}
