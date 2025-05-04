using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkTeaMe.Repositories.Enums;
using MilkTeaMe.Repositories.Models;
using MilkTeaMe.Services.BusinessObjects;
using MilkTeaMe.Services.Interfaces;
using MilkTeaMe.Web.Models.Requests;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MilkTeaMe.Web.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Route("Customer/[controller]")]
    [Authorize(Roles = "customer")]
    public class OrdersController : Controller
	{
		private readonly IOrderService _orderService;
		private readonly IVNPayService _vnPayService;
		private readonly IPaymentService _paymentService;

		public OrdersController(IOrderService orderService, IVNPayService vpnService, IPaymentService paymentService)
		{
			_orderService = orderService;
			_vnPayService = vpnService;
			_paymentService = paymentService;
		}

		[HttpGet("")]
		[HttpGet("Index")]
		public IActionResult Index()
		{
			return View();
		}

        [HttpGet("order-history")]
        public async Task<IActionResult> OrderHistory(string? status, int? page)
        {
			string? email = User.FindFirstValue(ClaimTypes.Name);
			if (email == null)
			{
				return RedirectToAction("Login", "Auth", new { area = "" });
			}

            int pageSize = 5;
            int pageNumber = page ?? 1;

            List<Order> orders = new List<Order>();

			try
			{
                var data = await _orderService.GetUserOrderHistory(email, status, pageNumber, pageSize);
				orders = data.Item1.ToList();
                ViewBag.TotalItems = data.Item2;
            }
			catch (Exception)
			{
				//TODO: user not found
				throw;
			}

            ViewBag.CurrentStatus = status?.ToLower() ?? "all";
            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageSize = pageSize;

            return View(orders);
        }

        [HttpGet("Success/{id}")]
		public async Task<IActionResult> Success(int id)
		{
			try
			{
				PaymentModel model = await _paymentService.GetPaymentInfo(id);
				return View(model);
			}
			catch (Exception)
			{
				// TODO: fix this
				throw;
			}	
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
