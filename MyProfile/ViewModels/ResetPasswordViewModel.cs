using System.ComponentModel.DataAnnotations;

namespace MyProfile.ViewModels
{
	public class ResetPasswordViewModel : NewPasswordViewModel
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		public string Token { get; set; }
	}
}
