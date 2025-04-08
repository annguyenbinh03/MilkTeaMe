using Microsoft.AspNetCore.Mvc;
using MilkTeaMe.Services.Interfaces;
using MilkTeaMe.Web.Models.Requests;

namespace MilkTeaMe.Web.Controllers
{
	public class AuthController : Controller
	{
        private readonly IAuthService _accountMemberService;

        public AuthController(IAuthService accountMemberService)
        {
            _accountMemberService = accountMemberService;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {

            var user = await _accountMemberService.Login(request.Username, request.Password);

            if (user != null)
            {
                CookieOptions cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddMinutes(30);
                HttpContext.Response.Cookies.Append("Email", user.Username, cookieOptions);
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password.");
            }
            return View(request);
        }

        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("Email");
            return RedirectToAction("Login", "Auth");
        }
    }
}
