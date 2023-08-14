using System.ComponentModel.DataAnnotations;

namespace MyProfile.ViewModels
{
	public class ChangePasswordViewModel : NewPasswordViewModel
	{
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Current Password")]
		public string CurrentPassword { get; set; }
	}
}
