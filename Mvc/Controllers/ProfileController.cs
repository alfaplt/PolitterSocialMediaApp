using Core.Entities;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mvc.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    public class ProfileController : Controller
    {
		private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public ProfileController(UserManager<AppUser> user, AppDbContext context)
        {
            _userManager = user;
            _context = context;
        }

        [Route("{userName}")]
        public async Task<IActionResult> Index(string userName)
        {
            AppUser user = _userManager.Users
                .Where(x => x.UserName == userName)
                .Include(x => x.Posts)
                .Include(x => x.Favorites)
                .Include(x => x.Comments)
                .Include(x => x.Followeds)
                .Include(x => x.Followings)
                .AsSplitQuery() // for more performance
                .SingleOrDefault();

            var posts = _context.Posts
                .Where(x => x.AppUser == user)
                .Include(x => x.Comments)
                .Include(x => x.Favorites) 
                .AsSplitQuery()
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            UserPosts userPosts = new()
            {
                AppUser = user,
                Posts = posts
            };


            var follows = _context.Follows.Include(x => x.Following).Include(x => x.Followed).ToList();

            if(User.Identity.Name != null)
            {
                int authUserId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;

                if (user != null)
                {
                    var follow = _context.Follows.FirstOrDefault(x => x.FollowingId == authUserId && x.FollowedId == user.Id);

                    if (follows.Contains(follow))
                    {
                        ViewData["followLink"] = "Takibi Bırak";
                    }
                    else
                    {
                        ViewData["followLink"] = "Takip Et";
                    }
                }
            }
            ViewData["followLink"] = "Takip Et";




            return View(userPosts);
        }

        [Route("_ListUsers")]
        public async Task<IActionResult> _ListUsersAsync()
        {
            var users = _userManager.Users.ToList();

            if(User.Identity.IsAuthenticated)
            {
                users.Remove(await _userManager.FindByNameAsync(User.Identity.Name));
            }
            
            return PartialView(users);
        }
    }
}
