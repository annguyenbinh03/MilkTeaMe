using Microsoft.AspNetCore.Mvc;
using MilkTeaMe.Services.BusinessObjects;
using MilkTeaMe.Web.Models.Requests;
using Newtonsoft.Json;

namespace MilkTeaMe.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
