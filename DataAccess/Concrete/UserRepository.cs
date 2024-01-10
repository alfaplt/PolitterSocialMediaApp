using Core.Entities;
using DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class UserRepository : GenericRepository<AppUser>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<AppUser>> GetAllWithPostsAndCommentsAndFavsAsync()
        {
            return await _context.Users.Include(x => x.Posts).Include(x => x.Comments).Include(x => x.Favorites).ToListAsync();
        }

        public async Task<AppUser> GetWithPostsAndOthersByIdAsync(int id)
        {
            return await _context.Users.Include(x => x.Posts).Include(x => x.Comments).Include(x => x.Favorites).SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
