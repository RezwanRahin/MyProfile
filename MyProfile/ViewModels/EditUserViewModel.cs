using System.ComponentModel.DataAnnotations;
using MyProfile.Models;
using Microsoft.AspNetCore.Mvc;

namespace MyProfile.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }

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
        [Display(Name = "Username")]
        [Remote(action: "IsUsernameOfOther", controller: "Account", AdditionalFields = "Id")]
        public string UserName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        public List<string> Claims { get; set; }

        public IList<string> Roles { get; set; }
    }
}