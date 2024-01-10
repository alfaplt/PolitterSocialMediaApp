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
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context){ }

        public async Task<IEnumerable<Comment>> GetAllWithUserByPostIdAsync(int postId)
        {
            return await _context.Comments.Include(x => x.AppUser).Where(x => x.PostId == postId).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }
    }
}
