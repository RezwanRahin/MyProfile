using System.ComponentModel.DataAnnotations;
using MyProfile.Models;
using Microsoft.AspNetCore.Mvc;

namespace MyProfile.ViewModels
{
    public class EditProfileViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailOfOther", controller: "Account", AdditionalFields = "Id")]
        public string Email { get; set; }

        [Required]
        [Remote(action: "IsUsernameOfOther", controller: "Account", AdditionalFields = "Id")]
        public string Username { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        public string? PhotoPath { get; set; }
        public IFormFile? Photo { get; set; }


        public string GetPhoto(Gender gender, string? photoPath)
        {
            var photo = "male.jpg";
            if (gender == Gender.Female)
            {
                photo = "female.jpg";
            }

            return "~/images/" + (photoPath ?? photo);
        }
    }
}