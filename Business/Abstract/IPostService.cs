using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IPostService 
    {
        Task<IEnumerable<Post>> GetAllWithUser();
        Task<Post> GetPostById(int id);
        Task<IEnumerable<Post>> GetPostsByUserId(int userId);
        Task<Post> CreatePost(Post newpost);
        Task UpdatePost(Post postToBeUpdated, Post post);
        Task DeletePost(Post post);
        Task<IEnumerable<Post>> GetAllWithUserAndComments();
        IQueryable<Post> GetAllWithUserAndCommentsForPagedList();
    }
}
