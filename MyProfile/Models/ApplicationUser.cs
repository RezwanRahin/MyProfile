using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MyProfile.Models
{
	[Index(nameof(Email), IsUnique = true)]
	[Index(nameof(UserName), IsUnique = true)]
	public class ApplicationUser : IdentityUser
	{
		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		public Gender Gender { get; set; }

		[Required]
		public DateTime DOB { get; set; }

		public string? PhotoPath { get; set; }
	}
}
