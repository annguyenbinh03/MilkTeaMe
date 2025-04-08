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
	public class EmployeeService : IEmployeeService
	{
		private readonly IUnitOfWork _unitOfWork;

		public EmployeeService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task Create(Employee request)
		{
			await _unitOfWork.EmployeeRepository.InsertAsync(request);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task Delete(int id)
		{
			var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
			if (employee != null)
			{
				employee.Status = EmployeeStatus.inactive.ToString();
				_unitOfWork.EmployeeRepository.Update(employee);
				await _unitOfWork.SaveChangesAsync();
			}
		}

		public async Task<Employee?> GetEmployee(int id)
		{
			return await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
		}

		public async Task<(IEnumerable<Employee>, int)> GetEmployees(string? search, int? page = null, int? pageSize = null)
		{
			var (employees, totalItems) = await _unitOfWork.EmployeeRepository.GetAsync(null, null, null);
			return (employees, totalItems);
		}

		public async Task Update(Employee request)
		{
			_unitOfWork.EmployeeRepository.Update(request);
			await _unitOfWork.SaveChangesAsync();
		}
	}
}
