using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mvc.Controllers
{
    [AllowAnonymous]
	public class HomeController : Controller
    {
		public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Posts");
            }
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
    }
}
