using MilkTeaMe.Repositories.Enums;
using MilkTeaMe.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<(IEnumerable<Order>, int)> GetUserOrderHistory(User user, OrderStatus? orderStatus , int? page = null, int? pageSize = null);
        Task<(IEnumerable<Order>, int)> GetOrderHistory(string? search, OrderStatus? orderStatus , int? page = null, int? pageSize = null);
    }
}
