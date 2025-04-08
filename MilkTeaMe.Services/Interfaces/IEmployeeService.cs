using MilkTeaMe.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<(IEnumerable<Employee>, int)> GetEmployees(string? search, int? page = null, int? pageSize = null);
        Task<Employee?> GetEmployee(int id);
        Task Create(Employee request);
        Task Update(Employee request);
        Task Delete(int id);
    }
}
