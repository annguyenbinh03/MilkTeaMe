using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MilkTeaMe.Repositories.DbContexts;
using MilkTeaMe.Repositories.Models;
using MilkTeaMe.Services.Interfaces;

namespace MilkTeaMe.Web.Controllers.Manager
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: Orders
        public async Task<IActionResult> Index(int? page,
           string? search)
        {
            int pageSize = 4;
            int pageNumber = page ?? 1;

            var (orders, totalItems) = await _orderService.GetOrders(search, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.Search = search;
            return View(orders);
        }
    }
}
