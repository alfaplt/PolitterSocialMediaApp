using Business.Abstract;
using Core.Entities;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
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

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            //var posts = await _postService.GetAllWithUserAndComments();
            var posts = _postService.GetAllWithUserAndCommentsForPagedList().ToPagedList(page, pageSize);
            string authUserName = User.Identity.Name;
            int authUserId = (await _userManager.FindByNameAsync(authUserName)).Id;
         
            AppUser user = _context.Users.Include(x => x.Followeds).Where(x => x.Id == authUserId).FirstOrDefault();
            var follows = _context.Follows.Include(x => x.Following).Include(x => x.Followed).ToList();

            #region For follow suggestions 
            // Code review needed for this section

            var users = _userManager.Users.ToList();
            Random randomnbr = new();
            var x = users.Count;

            int authUserIndex = users.IndexOf(user);

            List<int> values = new List<int>();

            int nbr1 = randomnbr.Next(x);

            int nbr2 = randomnbr.Next(x);

            while (nbr1 == authUserIndex)
            {
                nbr1 = randomnbr.Next(x);
            }
            values.Add(nbr1);

            while (nbr1 == nbr2 || nbr2 == authUserIndex)
            {
                nbr2 = randomnbr.Next(x);
            }
            values.Add(nbr2);

            int nbr3 = randomnbr.Next(x);

            while (nbr3 == nbr2 || nbr3 == nbr1 || nbr3 == authUserIndex)
            {
                nbr3 = randomnbr.Next(x);
            }
            values.Add(nbr3);


            ViewBag.randomUser1 = $"{users[nbr1].FirstName} {users[nbr1].LastName}";
            ViewBag.randomUser2 = $"{users[nbr2].FirstName} {users[nbr2].LastName}";
            ViewBag.randomUser3 = $"{users[nbr3].FirstName} {users[nbr3].LastName}";

            ViewBag.userName1 = users[nbr1].UserName;
            ViewBag.userName2 = users[nbr2].UserName;
            ViewBag.userName3 = users[nbr3].UserName;

            ViewBag.userId1 = users[nbr1].Id;
            ViewBag.userId2 = users[nbr2].Id;
            ViewBag.userId3 = users[nbr3].Id;

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
