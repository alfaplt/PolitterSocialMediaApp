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
    public class FollowService : IFollowService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FollowService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Follow(Follow follow)
        {
            await _unitOfWork.Follows.AddAsync(follow);
            await _unitOfWork.CommitAsync();
        }

        public async Task UnFollow(Follow unfollow)
        {
            _unitOfWork.Follows.Remove(unfollow);
            await _unitOfWork.CommitAsync();
        }
    }
}
