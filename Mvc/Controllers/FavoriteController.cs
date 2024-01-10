using Core.Entities;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Mvc.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly AppDbContext _context;
		public FavoriteController(AppDbContext context)
		{
			_context = context;
		}
		public IActionResult Favorite(int Id)
        {
           
            var userName = User.Identity.Name;
            AppUser user = _context.Users.Where(x => x.UserName == userName).FirstOrDefault();

            var favs = _context.Favorites.Include(x => x.User).Include(x => x.Post).ToList();

            var fav = _context.Favorites
                .FirstOrDefault(x => x.AppUserId == user.Id && x.PostId == Id);

            if (favs.Contains(fav))
            {
                _context.Remove(fav);
                _context.SaveChanges();
            }
            else
            {
                Favorite newFav = new Favorite()
                {
                    AppUserId = user.Id,
                    PostId = Id
                };

                _context.Add(newFav);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Posts");
        }
    }
}
