using Microsoft.AspNetCore.Mvc;

namespace MyProfile.Controllers
{
	public class AdministrationController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
