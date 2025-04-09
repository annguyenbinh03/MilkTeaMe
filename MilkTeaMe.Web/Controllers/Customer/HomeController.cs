using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using MilkTeaMe.Repositories.Enums;
using MilkTeaMe.Services.Implementations;
using MilkTeaMe.Services.Interfaces;
using MilkTeaMe.Web.Models.Responses;
using System.Drawing.Printing;
using System.Text.Json;

namespace MilkTeaMe.Web.Controllers.Customer
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            var (products, totalItems) = await _productService.GetMilkTeas(null, null, null);
            var (toppings, totalTopping) = await _productService.GetToppings(null, null, null); 

            ViewBag.Toppings = toppings;
            return View(products);
        }
    }
}
