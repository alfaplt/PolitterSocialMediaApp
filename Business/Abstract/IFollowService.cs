using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IFollowService
    {
        Task Follow(Follow follow);
        Task UnFollow(Follow unfollow);
    }
}
