using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CinemaApp.Common.EntityValidationConstants.Group;

namespace CinemaApp.Data.Models
{
    public class Group
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(DescriptionMinLength)]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        [MinLength(LocationMinLength)]
        [MaxLength(LocationMaxLength)]
        public string Location { get; set; } = null!;

        public DateTime CreatedDate { get; set; }
        
        public Guid AdminId { get; set; }
        [ForeignKey(nameof(AdminId))]        
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;

        public ICollection<Membership> Memberships { get; set; } = new HashSet<Membership>();

        public ICollection<Event> Events { get; set; } = new HashSet<Event>();
    }
}

