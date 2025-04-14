using MilkTeaMe.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.Interfaces
{
    public interface IUserService
    {
        Task<(IEnumerable<User>, int)> GetEmployees(string? search, int? page = null, int? pageSize = null);
        Task<User?> GetEmployee(int id);
        Task Create(User request);
        Task Update(User request);
        Task Delete(int id);
    }
}
