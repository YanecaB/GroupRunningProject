using System;
using System.ComponentModel.DataAnnotations;
using CinemaApp.Data.Models;

namespace CinemaApp.Data.Seeding.DTOs
{
	public class ImportMembershipDto
	{
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string ApplicationUserId { get; set; } = null!;

        [Required]
        public string GroupId { get; set; } = null!;

        public DateTime JoinDate { get; set; } = DateTime.Today;
    }
}

