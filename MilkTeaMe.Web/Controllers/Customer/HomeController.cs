using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using MilkTeaMe.Repositories.Enums;
using MilkTeaMe.Services.Implementations;
using MilkTeaMe.Services.Interfaces;
using MilkTeaMe.Web.Infrastructure.Extensions;
using MilkTeaMe.Web.Models.Requests;
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

        [HttpPost("cart/add")]
        public IActionResult AddToCart([FromBody] ProductToCartRequest request)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

            var existing = cart.FirstOrDefault(c =>
                c.ProductId == request.ProductId &&
                c.Size == request.Size &&
                Enumerable.SequenceEqual(c.ToppingIds.OrderBy(x => x), (request.Toppings ?? new()).OrderBy(x => x)));

            if (existing != null)
            {
                existing.Quantity += request.Quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = request.ProductId,
                    Size = request.Size,
                    Quantity = request.Quantity,
                    ToppingIds = request.Toppings ?? new()
                });
            }

            HttpContext.Session.SetObject("Cart", cart);
            return Ok();
        }
    }
}
