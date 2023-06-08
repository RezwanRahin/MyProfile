using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MyProfile.Models;

namespace MyProfile.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailOfOther", controller: "Account")]
        public string Email { get; set; }

        [Required]
        [Remote(action: "IsUsernameInUse", controller: "Account")]
        public string Username { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}