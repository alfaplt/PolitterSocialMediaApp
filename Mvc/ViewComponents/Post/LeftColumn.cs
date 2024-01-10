using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Mvc.ViewComponents.Post
{
    public class LeftColumn : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public LeftColumn(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var userName = User.Identity.Name;
            AppUser user = _userManager.Users.Where(x => x.UserName == userName).FirstOrDefault();
            return View(user);
        }
    }
}
