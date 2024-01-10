using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserService 
    {
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<IEnumerable<AppUser>> GetAllUsersWithPosts();
        Task<AppUser> GetUserByIdWithPosts(int id);
        Task<AppUser> GetUserById(int id);
        Task<AppUser> CreateUser(AppUser newUser);
        Task UpdateUser(AppUser userToBeUpdated, AppUser appUser);
        Task DeleteUser(AppUser appUser);

    }
}
