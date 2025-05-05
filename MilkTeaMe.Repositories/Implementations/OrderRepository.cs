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

        public async Task<(IEnumerable<Order>, int)> GetOrderHistory(string? search , OrderStatus? orderStatus, int? page = null, int? pageSize = null)
        {
            IQueryable<Order> query = _dbSet;

            query = query.Include(o => o.OrderDetails)
                            .ThenInclude(od => od.Product)
                        .Include(o => o.OrderDetails)
                            .ThenInclude(od => od.Size)
                        .Include(o => o.User);

            if (orderStatus != null)
            {
                query = query.Where(o => o.Status == orderStatus.ToString());
            }

            if (search != null)
            {
                int? searchId = int.TryParse(search, out int id) ? id : null;
                var loweredSearch = search.ToLower();

                query = query.Where(o =>
                    o.User.Username.ToLower().Contains(loweredSearch) ||
                    o.User.Email.ToLower().Contains(loweredSearch) ||
                    (searchId != null && o.Id == searchId.Value));
            }

            int totalItems = await query.CountAsync();

            query = query.OrderByDescending(o => o.CreatedAt);

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return (await query.ToListAsync(), totalItems);
        }

        public async Task<(IEnumerable<Order>, int)> GetUserOrderHistory(User user, OrderStatus? orderStatus, int? page = null, int? pageSize = null)
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

            query = query.Where(o => o.User == user);

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
