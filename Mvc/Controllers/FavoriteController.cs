using Business.Abstract;
using Core.Entities;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFavoriteService _favoriteService;
		public FavoriteController(AppDbContext context, IFavoriteService favoriteService)
		{
			_context = context;
			_favoriteService = favoriteService;
		}
        public async Task<IActionResult> Favorite(int Id)
        {

            var userName = User.Identity.Name;
            AppUser user = _context.Users.Where(x => x.UserName == userName).FirstOrDefault();
            Post postToFav = _context.Posts.Where(x => x.Id == Id).FirstOrDefault();

            var favs = _context.Favorites.Include(x => x.User).Include(x => x.Post).ToList();

            var fav = _context.Favorites.FirstOrDefault(x => x.AppUserId == user.Id && x.PostId == Id);

            if(postToFav != null)
            {
                if (favs.Contains(fav))
                {
                    await _favoriteService.Unfavorite(fav);
                }
                else
                {
                    Favorite newFav = new Favorite()
                    {
                        AppUserId = user.Id,
                        PostId = Id
                    };

                    await _favoriteService.Favorite(newFav);
                }
            }
            
            return RedirectToAction("index", "posts");
        }
    }
}
