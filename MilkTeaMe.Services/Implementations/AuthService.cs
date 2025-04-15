using MilkTeaMe.Repositories.Enums;
using MilkTeaMe.Repositories.Models;
using MilkTeaMe.Repositories.UnitOfWork;
using MilkTeaMe.Services.BusinessObjects;
using MilkTeaMe.Services.Exceptions;
using MilkTeaMe.Services.Interfaces;
using MilkTeaMe.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> GetOrCreateAccountByEmail(string email)
        {
           User? user = await _unitOfWork.UserRepository.FindOneAsync(filter: filter => filter.Email == email);

            if(user == null)
            {
                User newUser = new User
                {
                    Username = email,
                    Role = UserRole.customer.ToString(),
                    Email = email,
                    Status = UserStatus.active.ToString(),
                    CreatedAt = TimeZoneUtil.GetCurrentTime(),
                    UpdatedAt = TimeZoneUtil.GetCurrentTime(),
                };

                await _unitOfWork.UserRepository.InsertAsync(newUser);
                await _unitOfWork.SaveChangesAsync();
                return newUser;
            }
            return user;
        }

        public async Task<User?> Login(string username, string password)
        {
            return await _unitOfWork.UserRepository.FindOneAsync(
                    filter: ac => ac.Username == username && ac.Password == password);
        }

		public async Task<User> Register(UserModel model)
		{

            User? existingUser = await _unitOfWork.UserRepository.FindOneAsync(filter: u => u.Username == model.Username);

            if (existingUser != null) {
                throw new UserAlreadyExistsByUsernameException();
			}

			existingUser = await _unitOfWork.UserRepository.FindOneAsync(filter: u => u.Email == model.Email);

			if (existingUser != null)
			{
				throw new UserAlreadyExistsByEmailException();
			}

			User user = new User
            {
                Username = model.Username,
                Password = model.Password,
                Role = UserRole.customer.ToString(),
                Email = model.Email,
                Status = UserStatus.active.ToString(),
                CreatedAt = TimeZoneUtil.GetCurrentTime(),
                UpdatedAt = TimeZoneUtil.GetCurrentTime(),
            };
            await _unitOfWork.UserRepository.InsertAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return user;
		}
	}
}
