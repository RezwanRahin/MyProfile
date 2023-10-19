using MyProfile.Models;

namespace MyProfile.ViewModels
{
	public class ProfileViewModel
	{
		public string UserName { get; set; }
		public string PhotoPath { get; set; }

		public ProfileViewModel(ApplicationUser user)
		{
			UserName = user.UserName;
			PhotoPath = GetPhotoPath(user);
		}

		private static string GetPhotoPath(ApplicationUser user)
		{
			var photo = "male.jpg";
			if (user.Gender == Gender.Female)
			{
				photo = "female.jpg";
			}
			return "~/images/" + (user.PhotoPath ?? photo);
		}
	}
}
