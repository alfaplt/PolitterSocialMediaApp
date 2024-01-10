using Business.Abstract;
using Core.Entities;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Post> CreatePost(Post newpost)
        {
            await _unitOfWork.Posts.AddAsync(newpost);
            await _unitOfWork.CommitAsync();
            return newpost;
        }

        public async Task DeletePost(Post post)
        {
            _unitOfWork.Posts.Remove(post);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Post>> GetAllWithUser()
        {
            return await _unitOfWork.Posts.GetAllWithUserAsync();
        }

        public async Task<Post> GetPostById(int id)
        {
            return await _unitOfWork.Posts.GetWithUserByIdAsync(id);
        }

        public async Task<IEnumerable<Post>> GetPostsByUserId(int userId)
        {
            return await _unitOfWork.Posts.GetAllWithUserByUserIdAsync(userId);
        }

        public async Task UpdatePost(Post postToBeUpdated, Post post)
        {
            postToBeUpdated.Content = post.Content;
            await _unitOfWork.CommitAsync();     
        }
        public async Task<IEnumerable<Post>> GetAllWithUserAndComments()
        {
            return await _unitOfWork.Posts.GetAllWithUserAndCommentsAsync();
        }

    }
}
