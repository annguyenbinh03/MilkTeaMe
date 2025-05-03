using Microsoft.EntityFrameworkCore;
using MilkTeaMe.Repositories.Enums;
using MilkTeaMe.Repositories.Models;
using MilkTeaMe.Repositories.UnitOfWork;
using MilkTeaMe.Services.BusinessObjects;
using MilkTeaMe.Services.Exceptions;
using MilkTeaMe.Services.Interfaces;
using MilkTeaMe.Services.Utils;
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

		public async Task<int> Create(List<CartItemModel> cardItems)
		{
			Order order = new Order();
			order.Status = OrderStatus.pending.ToString();
			order.CreatedAt = TimeZoneUtil.GetCurrentTime();
			order.UpdatedAt = TimeZoneUtil.GetCurrentTime();
			await _unitOfWork.OrderRepository.InsertAsync(order);

			decimal totalMoney = 0;

			foreach (var item in cardItems)
			{

				decimal totalProductMoney = 0;

				var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId) ?? throw new Exception("Product not found with Id:" + item.ProductId);

				OrderDetail parentOrderDetail;

				if (!string.IsNullOrEmpty(item.Size)) //Check if the product is typeOf milktea or not
				{
					if (!Enum.TryParse<ProductSizeEnum>(item.Size, out var sizeEnum))
					{
						throw new Exception($"Kích cỡ sản phẩm không hợp lệ: {item.Size}");
					}
					int sizeValue = (int)sizeEnum;

					var productSize = await _unitOfWork.ProductSizeRepository.FindOneAsync(filter: ps => ps.SizeId == sizeValue && ps.Product == product) ?? throw new Exception("Not found product size:" + sizeValue + " - productId: " + product.Id);

					parentOrderDetail = new OrderDetail()
					{
						Order = order,
						ProductId = product.Id,
						Quantity = item.Quantity,
						Price = productSize.Price,
						SizeId = sizeValue,
					};

					totalProductMoney += productSize.Price;

					await _unitOfWork.OrderDetailRepository.InsertAsync(parentOrderDetail);

					foreach (var toppingId in item.Toppings)
					{
						var toppingProduct = await _unitOfWork.ProductRepository.FindOneAsync(filter: p => p.Id == toppingId) ?? throw new Exception("Topping not found or inactive with Id:" + toppingId);

						var childOrderDetail = new OrderDetail()
						{
							Order = order,
							ProductId = toppingProduct.Id,
							Quantity = 0, //default value for topping, it's quantity will depend on parent quantity
							Price = toppingProduct.Price ?? 0,
							Parent = parentOrderDetail,
						};

						totalProductMoney += toppingProduct.Price ?? 0;

						await _unitOfWork.OrderDetailRepository.InsertAsync(childOrderDetail);
					}
				}
				else
				{
					parentOrderDetail = new OrderDetail()
					{
						Order = order,
						ProductId = product.Id,
						Quantity = item.Quantity,
						Price = product.Price ?? 0
					};

					totalProductMoney += product.Price ?? 0;

					await _unitOfWork.OrderDetailRepository.InsertAsync(parentOrderDetail);
				}

				totalMoney += totalProductMoney * item.Quantity;
			}

			order.TotalPrice = totalMoney;
			await _unitOfWork.SaveChangesAsync();
			return order.Id;
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

		public async Task<(IEnumerable<Order>, int)> GetUserOrderHistory(string email)
		{
			User user = await _unitOfWork.UserRepository.FindOneAsync(filter: u => u.Email == email) ?? throw new UserNotFound();

			var (orders, totalItem) = await _unitOfWork.OrderRepository.GetOrderHistory(user);

			//remove topping on the list (topping would be children in parent order detail)
            foreach (var order in orders)
            {
                var allDetails = order.OrderDetails;
                order.OrderDetails = allDetails
                    .Where(d => d.ParentId == null)
                    .ToList();
            }

			return (orders, totalItem);
        }
	}
}
