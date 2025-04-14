using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MilkTeaMe.Repositories.Enums;
using MilkTeaMe.Services.Interfaces;
using MilkTeaMe.Web.Models.Requests;
using System.Security.Claims;

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
            var user = HttpContext.User;

            if (user.Identity.IsAuthenticated)
            {
                string role = user.FindFirstValue(ClaimTypes.Role);

                if (role == UserRole.manager.ToString())
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Manager" });
                }
                else if (role == UserRole.customer.ToString())
                {
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {

            var user = await _accountMemberService.Login(request.Username, request.Password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = request.RememberMe,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                    });

                if (user.Role.Equals(UserRole.customer.ToString()))
                {
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                } else if (user.Role.Equals(UserRole.manager.ToString()))
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Manager" });
                }

                return RedirectToAction("Index", "Dashboard", new { area = "Manager" });
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password.");
            }
            return View(request);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
