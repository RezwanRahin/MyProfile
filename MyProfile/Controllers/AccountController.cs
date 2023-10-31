using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProfile.Extensions;
using MyProfile.Models;
using MyProfile.ViewModels;

namespace MyProfile.Controllers
{
	[AllowAnonymous]
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IWebHostEnvironment _hostEnvironment;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
			IWebHostEnvironment hostEnvironment)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_hostEnvironment = hostEnvironment;
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

		[HttpGet]
		public IActionResult AccessDenied()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterUserViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = new ApplicationUser
			{
				FirstName = model.FirstName,
				LastName = model.LastName,
				Email = model.Email,
				UserName = model.Username,
				Gender = model.Gender,
				DOB = model.DOB
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				await _signInManager.SignInAsync(user, isPersistent: false);
				return RedirectToAction("Index", "Home");
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
			return View(model);
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByNameAsync(model.Identifier) ?? await _userManager.FindByEmailAsync(model.Identifier);

				if (user != null)
				{
					var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
					if (result.Succeeded)
					{
						if (Url.IsLocalUrl(returnUrl))
						{
							return Redirect(returnUrl);
						}
						return RedirectToAction("Index", "Home");
					}
				}

				ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
			}

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		[Route("[controller]/[action]/{username}")]
		public async Task<IActionResult> Details(string username)
		{
			var user = await _userManager.FindByNameAsync(username);

			if (user == null)
			{
				Response.StatusCode = 404;
				ViewBag.ErrorMessage = $"User with Username = {username} cannot be found";
				return View("NotFound");
			}

			var model = new UserDetailsViewModel
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				Username = user.UserName,
				Gender = user.Gender,
				DOB = user.DOB,
				PhotoPath = user.PhotoPath
			};

			var signedInUser = await _userManager.GetUserAsync(User);

			if (user == signedInUser)
			{
				model.AllowModifications = true;
			}

			return View(model);
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Edit(string username)
		{
			var user = await _userManager.FindByNameAsync(username);
			var signedInUser = await _userManager.GetUserAsync(User);

			if (user != signedInUser)
			{
				return RedirectToAction("AccessDenied");
			}

			if (user == null)
			{
				Response.StatusCode = 404;
				ViewBag.ErrorMessage = $"User with Username = {username} cannot be found";
				return View("NotFound");
			}

			var model = new EditProfileViewModel
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				Username = user.UserName,
				Gender = user.Gender,
				DOB = user.DOB,
				PhotoPath = user.PhotoPath
			};

			return View(model);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Edit(EditProfileViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.FindByIdAsync(model.Id);
			user.FirstName = model.FirstName;
			user.LastName = model.LastName;
			user.Email = model.Email;
			user.UserName = model.Username;
			user.DOB = model.DOB;

			string? oldFilePath = null;

			if (model.Photo != null)
			{
				oldFilePath = user.PhotoPath;
				user.PhotoPath = await model.Photo.ProcessUploadedFile(_hostEnvironment);
			}

			var result = await _userManager.UpdateAsync(user);

			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}

				return View(model);
			}

			oldFilePath?.DeleteImageFile(_hostEnvironment);

			return RedirectToAction("Details", new { username = user.UserName });
		}

		[HttpGet]
		[Authorize]
		[Route("[controller]/[action]/{username}")]
		public async Task<IActionResult> ChangePassword(string username)
		{
			var user = await _userManager.FindByNameAsync(username);

			if (user == null)
			{
				Response.StatusCode = 404;
				ViewBag.ErrorMessage = $"User with Username = {username} cannot be found";
				return View("NotFound");
			}

			var signedInUser = await _userManager.GetUserAsync(User);

			if (user != signedInUser)
			{
				if (await _userManager.IsInRoleAsync(signedInUser, "Admin"))
				{
					return RedirectToAction("ResetPassword", "Administration", new { username = user.UserName });
				}

				return RedirectToAction("AccessDenied");
			}

			var model = new ChangePasswordViewModel { UserName = user.UserName };

			return View(model);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.FindByNameAsync(model.UserName);
			if (user == null)
			{
				Response.StatusCode = 404;
				ViewBag.ErrorMessage = $"User with Username = {model.UserName} cannot be found";
				return View("NotFound");
			}

			var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				return View();
			}

			await _signInManager.RefreshSignInAsync(user);

			ViewBag.Title = "Change Password";
			ViewBag.Username = user.UserName;
			ViewBag.Message = "User Password successfully changed.";

			return View("PasswordConfirmation");
		}
	}
}
