using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyProfile.Models;

namespace MyProfile.Controllers
{
	public class AdministrationController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public AdministrationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult ListUsers()
		{
			var users = _userManager.Users;
			return View(users);
		}
	}
}
