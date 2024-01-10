using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<IEnumerable<Post>> GetAllWithUserAsync();
        Task<Post> GetWithUserByIdAsync(int id);
        Task<IEnumerable<Post>> GetAllWithUserByUserIdAsync(int userId);
        Task<IEnumerable<Post>> GetAllWithUserAndCommentsAsync();
    }
}
