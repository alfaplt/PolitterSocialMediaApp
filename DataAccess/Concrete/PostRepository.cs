using Core.Entities;
using DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Post>> GetAllWithUserAsync()
        {
            return await _context.Posts.Include(x => x.AppUser).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllWithUserByUserIdAsync(int userId)
        {
            return await _context.Posts.Include(x => x.AppUser).Where(x => x.AppUserId == userId).ToListAsync();
        }

        public async Task<Post> GetWithUserByIdAsync(int id)
        {
            return await _context.Posts.Include(x => x.AppUser).Include(x => x.Comments).Include(x => x.Favorites).AsSplitQuery().SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IEnumerable<Post>> GetAllWithUserAndCommentsAsync()
        {
            return await _context.Posts.Include(x => x.AppUser).Include(x => x.Comments).Include(x => x.Favorites).AsSplitQuery().OrderByDescending(x => x.CreatedDate).ToListAsync();
        }
        public IQueryable<Post> GetAllWithUserAndCommentsForPagedList()
        {
            return _context.Posts.Include(x => x.AppUser).Include(x => x.Comments).Include(x => x.Favorites).AsSplitQuery().OrderByDescending(x => x.CreatedDate);
        }

    }
}
