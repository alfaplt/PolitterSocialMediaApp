using Business.Abstract;
using Core.Entities;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Controllers
{
    public class FollowController : Controller
    {
        private readonly IFollowService _followService;
        private readonly AppDbContext _context;
        public FollowController(AppDbContext context, IFollowService followService)
        {
            _context = context;
            _followService = followService;
        }
        public async Task<IActionResult> FollowUnFollow(int Id)
        {
            var userName = User.Identity.Name;
            AppUser user = _context.Users.Where(x => x.UserName == userName).FirstOrDefault();
            AppUser followedUser = _context.Users.Where(x => x.Id == Id).FirstOrDefault();

            var follows = _context.Follows.Include(x => x.Following).Include(x => x.Followed).ToList();

            var follow = _context.Follows.FirstOrDefault(x => x.FollowingId == user.Id && x.FollowedId == Id);

            if (followedUser != null)
            {
                if (follows.Contains(follow))
                {
                    await _followService.UnFollow(follow);
                }
                else
                {
                    Follow newFollow = new()
                    {
                        FollowingId = user.Id,
                        FollowedId = Id
                    };

                    await _followService.Follow(newFollow);
                }
            }   

            return RedirectToAction("index", "posts");
        }
    }
}
