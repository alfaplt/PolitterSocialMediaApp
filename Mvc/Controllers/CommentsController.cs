using Business.Abstract;
using Core.Entities;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<AppUser> _userManager;

        public CommentsController(ICommentService commentService, UserManager<AppUser> userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }

        public async Task<IActionResult> _ListComments(int Id)
        {
            ViewBag.Id = Id;
            var comments = await  _commentService.GetCommentsWithUserByPostId(Id);
            foreach (var comment in comments)
            {
                TimeSpan date = DateTime.Now.Subtract(comment.CreatedDate);

                if (date.TotalSeconds < 60)
                {
                    ViewData[comment.Id.ToString()] = $"{date.Seconds.ToString()} sn önce";
                }
                else if (date.TotalMinutes < 60)
                {
                    ViewData[comment.Id.ToString()] = $"{date.Minutes.ToString()} dk önce";
                }
                else if (date.TotalHours < 24)
                {
                    ViewData[comment.Id.ToString()] = $"{date.Hours.ToString()} sa önce";
                }
                else
                {
                    ViewData[comment.Id.ToString()] = $"{date.Days.ToString()} gün önce";
                }
            }
            return PartialView(comments);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int Id, Comment comment)
        {
			var userId = _userManager.Users.Where(x => x.UserName == User.Identity.Name).SingleOrDefault().Id;
            comment.AppUserId = userId;
            comment.CreatedDate = DateTime.Now;
            comment.PostId = Id;
            comment.Id = 0; // parametreden gelen postId değerini commentId'ye atadığı için sıfırladım
            
            await _commentService.CreateComment(comment);
            return RedirectToAction("Index", "Posts");
        }

        public async Task<IActionResult> Delete(Comment comment)
        {
            await _commentService.DeleteComment(comment);
            return RedirectToAction("Index", "Posts");
        }
    }
}
