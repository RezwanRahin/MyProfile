using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProfile.Models;

namespace MyProfile.Controllers
{
	[AllowAnonymous]
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpGet, HttpPost]
		public async Task<IActionResult> IsEmailInUse(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			return user == null ? Json(true) : Json($"Email {email} is already in use");
		}

		[HttpGet, HttpPost]
		public async Task<IActionResult> IsEmailOfOther(string email, string id)
		{
			var result = await _userManager.Users.AnyAsync(u => u.Email == email && u.Id != id);
			return result ? Json($"Email {email} is already in use") : Json(true);
		}
	}
}
