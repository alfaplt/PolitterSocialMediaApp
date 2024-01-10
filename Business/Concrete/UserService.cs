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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<AppUser> CreateUser(AppUser newUser)
        {
            await _unitOfWork.Users.AddAsync(newUser);
            await _unitOfWork.CommitAsync();
            return newUser;
        }

        public async Task DeleteUser(AppUser appUser)
        {
            _unitOfWork.Users.Remove(appUser);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await _unitOfWork.Users.GetAllAsync();     
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersWithPosts()
        {
            return await _unitOfWork.Users.GetAllWithPostsAndCommentsAndFavsAsync();
        }

        public async Task<AppUser> GetUserById(int id)
        {
            return await _unitOfWork.Users.GetByIdAsync(id);
        }

        public async Task<AppUser> GetUserByIdWithPosts(int id)
        {
            return await _unitOfWork.Users.GetWithPostsAndOthersByIdAsync(id);
        }

        public async Task UpdateUser(AppUser userToBeUpdated, AppUser appUser)
        {
            userToBeUpdated.FirstName = appUser.FirstName;
            userToBeUpdated.LastName = appUser.LastName;
            userToBeUpdated.UserName = appUser.UserName;
            userToBeUpdated.Email = appUser.Email;
            userToBeUpdated.PhoneNumber = appUser.PhoneNumber;
            userToBeUpdated.ProfilePicture = appUser.ProfilePicture;

            await _unitOfWork.CommitAsync();
        }

        public async Task Update(AppUser appUser)
        {
            _unitOfWork.Users.Update(appUser);
            await _unitOfWork.CommitAsync();
        }
    }
}
