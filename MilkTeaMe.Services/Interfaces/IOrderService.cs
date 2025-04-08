using MilkTeaMe.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.Interfaces
{
	public interface IOrderService
	{
		Task<(IEnumerable<Order>, int)> GetOrders(string? search, int? page = null, int? pageSize = null);
	}
}
