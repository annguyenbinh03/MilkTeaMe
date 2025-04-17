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
        Task<(IEnumerable<User>, int)> GetUsers(string? search, int? page = null, int? pageSize = null);
        Task<User?> GetUser(int id);
        Task<User?> GetUserByEmail(string email);
        Task ResetPasswordAsync(User user);
		Task Create(User request);
        Task Update(User request);
        Task Delete(int id);
    }
}
