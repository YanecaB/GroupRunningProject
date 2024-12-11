using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CinemaApp.Common.EntityValidationConstants.Group;
using static CinemaApp.Common.EntityValidationMessages.Group;

namespace CinemaApp.Data.Models
{
    public class Group
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = NameRequired)]
        [MinLength(NameMinLength, ErrorMessage = NameMinLengthMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = NameMaxLengthMessage)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = DescriptionRequired)]
        [MinLength(DescriptionMinLength, ErrorMessage = DescriptionMinLengthMessage)] 
        [MaxLength(DescriptionMaxLength, ErrorMessage = DescriptionMaxLengthMessage)] 
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = LocationRequired)]
        [MinLength(LocationMinLength, ErrorMessage = LocationMinLengthMessage)]
        [MaxLength(LocationMaxLength, ErrorMessage = LocationMaxLengthMessage)] 
        public string Location { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = AdminIdRequired)]
        public Guid AdminId { get; set; }

        [ForeignKey(nameof(AdminId))]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;

        public ICollection<Membership> Memberships { get; set; } = new HashSet<Membership>();

        public ICollection<Event> Events { get; set; } = new HashSet<Event>();
    }
}

