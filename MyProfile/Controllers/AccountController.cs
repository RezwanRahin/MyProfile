using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProfile.Models;
using MyProfile.ViewModels;

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

		[HttpGet, HttpPost]
		public async Task<IActionResult> IsUsernameInUse(string username)
		{
			var user = await _userManager.FindByNameAsync(username);
			return user == null ? Json(true) : Json($"Username '{username}' is already in use");
		}

		[HttpGet, HttpPost]
		public async Task<IActionResult> IsUsernameOfOther(string username, string id)
		{
			var result = await _userManager.Users.AnyAsync(u => u.UserName == username && u.Id != id);
			return result ? Json($"Username '{username}' is already in use") : Json(true);
		}

		[Route("Profiles")]
		public async Task<IActionResult> Index()
		{
			var users = await _userManager.Users.ToListAsync();
			var model = users.Select(u => new ProfileViewModel(u)).ToList();

			return View(model);
		}
	}
}
