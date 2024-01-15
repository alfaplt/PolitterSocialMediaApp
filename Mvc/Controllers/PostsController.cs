using Business.Abstract;
using Core.Entities;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        public PostsController(IPostService postService, UserManager<AppUser> userManager, AppDbContext context)
        {
            _postService = postService;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _postService.GetAllWithUserAndComments();
            string authUserName = User.Identity.Name;
            int authUserId = (await _userManager.FindByNameAsync(authUserName)).Id;
         
            AppUser user = _context.Users.Include(x => x.Followeds).Where(x => x.Id == authUserId).FirstOrDefault();
            var follows = _context.Follows.Include(x => x.Following).Include(x => x.Followed).ToList();

            #region For follow suggestions
            //sisteme giriş yapan kullanıcıyı da gösteriyor. fix it.
            var users = _userManager.Users.ToList();
            Random randomnbr = new();
            var x = users.Count;
            int nbr1 = randomnbr.Next(x);
            int nbr2 = randomnbr.Next(x);
            int nbr3 = randomnbr.Next(x);

            
            ViewBag.randomUser1 = users[nbr1].FirstName;
            ViewBag.randomUser2 = users[nbr2].FirstName;
            ViewBag.randomUser3 = users[nbr3].FirstName;

            ViewBag.usrPp1 = users[nbr1].ProfilePicture;
            ViewBag.usrPp2 = users[nbr2].ProfilePicture;
            ViewBag.usrPp3 = users[nbr3].ProfilePicture;

            #endregion


            foreach (var post in posts)
            {
                var follow = _context.Follows.FirstOrDefault(x => x.FollowingId == user.Id && x.FollowedId == post.AppUserId);

                // Unique bir ViewData key'i için böyle bir yol denendi.
                if (follows.Contains(follow))
                {
                    ViewData[post.Id.ToString()+ "x"] = "Takibi Bırak";
                }
                else
                {
                    ViewData[post.Id.ToString() + "x"] = "Takip Et";
                }

                #region TimeSpan for post created time
                TimeSpan date = DateTime.Now.Subtract(post.CreatedDate);

                if (date.TotalSeconds < 60)
                {
                    ViewData[post.Id.ToString()] = $"{date.Seconds.ToString()} sn önce";
                }
                else if (date.TotalMinutes < 60)
                {
                    ViewData[post.Id.ToString()] = $"{date.Minutes.ToString()} dk önce";
                }
                else if (date.TotalHours < 24)
                {
                    ViewData[post.Id.ToString()] = $"{date.Hours.ToString()} sa önce";
                }
                else
                {
                    ViewData[post.Id.ToString()] = $"{date.Days.ToString()} gün önce";
                }
                #endregion
            }

            return View(posts);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(Post post)
        {
            post.CreatedDate = DateTime.Now;
            var userName = User.Identity.Name;
            post.AppUserId = _userManager.Users.Where(x => x.UserName == userName).SingleOrDefault().Id;
            
            await _postService.CreatePost(post);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Post post)
        {
            await _postService.DeletePost(post);
            return RedirectToAction("Index");
        }

    }
}
