﻿using Microsoft.AspNetCore.Mvc;
using MilkTeaMe.Services.BusinessObjects;
using MilkTeaMe.Services.Interfaces;
using MilkTeaMe.Web.Models.Requests;
using Newtonsoft.Json;

namespace MilkTeaMe.Web.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Route("Customer/[controller]")]
	public class OrdersController : Controller
	{
		private readonly IOrderService _orderService;
		private readonly IVNPayService _vnPayService;

		public OrdersController(IOrderService orderService, IVNPayService vpnService)
		{
			_orderService = orderService;
			_vnPayService = vpnService;
		}

		[HttpGet("")]
		[HttpGet("Index")]
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet("Success")]
		public IActionResult Success()
		{
			return View();
		}

		[HttpPost("create")]
		//Create order and redirect to payemnt page
		public async Task<IActionResult> CreateOrder([FromBody] List<CartItemRequest> request)
		{
			List<CartItemModel> list = new List<CartItemModel>();

			try
			{
				foreach (var item in request)
				{
					int productId = item.ProductId;
					string size = item.Size;
					int quantity = item.Quantity;
					List<int> toppingIds = item.Toppings;

					list.Add(new CartItemModel
					{
						ProductId = productId,
						Size = size,
						Quantity = quantity,
						Toppings = toppingIds
					});
				}

				int orderId = await _orderService.Create(list);
				string redirectUrl = await _vnPayService.Charge(orderId);
				return Ok(new { redirectUrl });
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			return View();
		}

		[HttpGet("confirm")]
		public async Task<IActionResult> ConfirmPayment()
		{
			try
			{
				string redirectUrl = await _vnPayService.ConfirmPayment(Request);
				return Redirect(redirectUrl);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return View();
		}
	}
}
