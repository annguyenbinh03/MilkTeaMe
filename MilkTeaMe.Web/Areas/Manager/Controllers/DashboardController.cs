using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkTeaMe.Repositories.Enums;
using MilkTeaMe.Services.Interfaces;

namespace MilkTeaMe.Web.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Route("Manager/[controller]")]
    [Authorize(Roles = "manager")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        public async Task<IActionResult> Index()
        {
            var dailyRevenue = await _dashboardService.GetTotalRevenueByDay();
            var monthlyRevenue = await _dashboardService.GetTotalRevenueByMonth();
            var yearlyRevenue = await _dashboardService.GetTotalRevenueByYear();
            var products = await _dashboardService.GetHighLightProduct();

            ViewBag.DailyRevenue = dailyRevenue;
            ViewBag.MonthyRevenue = monthlyRevenue;
            ViewBag.AnnualRevenue = yearlyRevenue;
            ViewBag.Products = products;
            return View();
        }

        [HttpGet("sales-data")]
        public async Task<IActionResult> GetSalesData()
        {
            var salesData = await _dashboardService.GetSalesData();
            return Ok(salesData);
        }
    }
}
