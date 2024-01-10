using Business.Abstract;
using Core.Entities;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Comment> CreateComment(Comment newComment)
        {
            await _unitOfWork.Comments.AddAsync(newComment);
            await _unitOfWork.CommitAsync();
            return newComment;
        }

        public async Task DeleteComment(Comment comment)
        {
            _unitOfWork.Comments.Remove(comment);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Comment>> GetAllComments()
        {
            return await _unitOfWork.Comments.GetAllAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentsWithUserByPostId(int postId)
        {
            return await _unitOfWork.Comments.GetAllWithUserByPostIdAsync(postId);  
        }

        public async Task UpdateComment(Comment commentToBeUpdated, Comment comment)
        {
            commentToBeUpdated.Content = comment.Content;
            await _unitOfWork.CommitAsync();
        }
    }
}
