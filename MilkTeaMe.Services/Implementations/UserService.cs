using MilkTeaMe.Repositories.Enums;
using MilkTeaMe.Repositories.Models;
using MilkTeaMe.Repositories.UnitOfWork;
using MilkTeaMe.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.Implementations
{
	public class UserService : IUserService
	{
		private readonly IUnitOfWork _unitOfWork;

		public UserService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task Create(User request)
		{
			request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
			await _unitOfWork.UserRepository.InsertAsync(request);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task Delete(int id)
		{
			var employee = await _unitOfWork.UserRepository.GetByIdAsync(id);
			if (employee != null)
			{
				employee.Status = EmployeeStatus.inactive.ToString();
				_unitOfWork.UserRepository.Update(employee);
				await _unitOfWork.SaveChangesAsync();
			}
		}

		public async Task<User?> GetUser(int id)
		{
			return await _unitOfWork.UserRepository.GetByIdAsync(id);
		}

		public async Task<User?> GetUserByEmail(string email)
		{
			return await _unitOfWork.UserRepository.FindOneAsync(filter: u => u.Email == email);
		}

		public async Task<(IEnumerable<User>, int)> GetUsers(string? search, int? page = null, int? pageSize = null)
		{
			var (employees, totalItems) = await _unitOfWork.UserRepository.GetAsync(null, null, null);
			return (employees, totalItems);
		}

		public Task ResetPasswordAsync(User user)
		{
			throw new NotImplementedException();
		}

		public async Task Update(User request)
		{
			_unitOfWork.UserRepository.Update(request);
			await _unitOfWork.SaveChangesAsync();
		}
	}
}
