using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MilkTeaMe.Repositories.Models;
using MilkTeaMe.Services.Interfaces;

namespace MilkTeaMe.Web.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Route("Staff/[controller]")]
    [Authorize(Roles = "staff")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index(string? search, string? status, int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;
            List<Order> orders = new List<Order>();

            try
            {
                var data = await _orderService.GetOrderHistory(search, status, pageNumber, pageSize);
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
            ViewBag.Search = search;

            return View(orders);
        }
    }
}
