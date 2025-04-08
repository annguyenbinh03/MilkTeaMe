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
	public class OrderService : IOrderService
	{
		private readonly IUnitOfWork _unitOfWork;

		public OrderService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<(IEnumerable<Order>, int)> GetOrders(string? search, int? page = null, int? pageSize = null)
		{
			var (orders, totalItems) = await _unitOfWork.OrderRepository.GetAsync(page: page, pageSize: pageSize, includes: o => o.OrderDetails, orderBy: o => o.OrderByDescending(order => order.CreatedAt));

			foreach (var order in orders)
			{
				List<OrderDetail> details = new List<OrderDetail>();
				foreach (var orderDetail in order.OrderDetails)
				{
					var detail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(orderDetail.Id, od => od.Product, od => od.Size);
					if (detail != null)
						details.Add(detail);
				}
				order.OrderDetails = details;
			}
			return (orders, totalItems);
		}
	}
}
