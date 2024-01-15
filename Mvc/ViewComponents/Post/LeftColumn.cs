using Core.Entities;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Mvc.ViewComponents.Post
{
    public class LeftColumn : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public LeftColumn(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var userName = User.Identity.Name;
            AppUser user = _userManager.Users.Include(x => x.Followings).Include(x => x.Followeds).Where(x => x.UserName == userName).FirstOrDefault();
            return View(user);
        }
    }
}
