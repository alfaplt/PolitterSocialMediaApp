using Business.Abstract;
using Core.Entities;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Mvc.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    public class ProfileController : Controller
    {
		private readonly UserManager<AppUser> _user;

		public ProfileController(UserManager<AppUser> user)
		{
			_user = user;
		}

		[Route("{userName}")]
        public IActionResult Index(string userName)
        {
            AppUser user = _user.Users
                .Include(x => x.Posts)
                .Include(x => x.Favorites)
                .Include(x => x.Comments)
                .Where(x => x.UserName == userName)
                .FirstOrDefault();    

            return View(user);
        }
    }
}
