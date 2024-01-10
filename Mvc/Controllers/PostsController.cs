using Business.Abstract;
using Business.Concrete;
using Core.Entities;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Schema;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly UserManager<AppUser> _userManager;
        public PostsController(IPostService postService, UserManager<AppUser> userManager)
        {
            _postService = postService;
            _userManager = userManager;
        }

        // business'a taşı

        public async Task<IActionResult> Index()
        {
            var posts = await _postService.GetAllWithUserAndComments();

            //For follow suggestions
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

            foreach (var post in posts)
            {
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
