using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetCommentsWithUserByPostId(int postId);
        Task<Comment> CreateComment(Comment newComment);
        Task UpdateComment(Comment commentToBeUpdated, Comment comment);
        Task DeleteComment(Comment comment);
        Task<IEnumerable<Comment>> GetAllComments();
    }
}
