using Microsoft.EntityFrameworkCore;
using MilkTeaMe.Repositories.Enums;
using MilkTeaMe.Repositories.Interfaces;
using MilkTeaMe.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Repositories.Implementations
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(DbContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<Order>, int)> GetOrderHistory(User user, OrderStatus? orderStatus, int? page = null, int? pageSize = null)
        {
            IQueryable<Order> query = _dbSet;


            query = query.Include(o => o.OrderDetails)
                            .ThenInclude(od => od.Product)
                        .Include(o => o.OrderDetails)
                            .ThenInclude(od => od.Size);

            if (orderStatus != null)
            {
                query = query.Where(o => o.Status == orderStatus.ToString());
            }

            int totalItems = await query.CountAsync();

            query = query.OrderByDescending(o => o.CreatedAt);

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return (await query.ToListAsync(), totalItems);
        }
    }
}
