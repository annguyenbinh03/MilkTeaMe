﻿using MilkTeaMe.Repositories.Models;
using MilkTeaMe.Services.BusinessObjects;
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
		/// <summary>
		/// Create order and return order id
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<int> Create(List<CartItemModel> cardItems);
	}
}
