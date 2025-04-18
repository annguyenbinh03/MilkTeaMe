using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MilkTeaMe.Repositories.Enums;
using MilkTeaMe.Services.BusinessObjects;
using MilkTeaMe.Services.Exceptions;
using MilkTeaMe.Services.Interfaces;
using MilkTeaMe.Web.Models.Requests;
using System.Security.Claims;

namespace MilkTeaMe.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userSerivce;
        private readonly IResetPasswordService _resetPasswordService;

		public AuthController(IAuthService accountMemberService, IUserService userService, IResetPasswordService resetPasswordService)
        {
            _authService = accountMemberService;
            _userSerivce = userService;
            _resetPasswordService = resetPasswordService;
        }
        public IActionResult Login()
        {
            if (TempData["Notification"] != null)
            {
                ViewBag.Notification = TempData["Notification"];
            }

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
            var user = await _authService.Login(request.Username, request.Password);

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
                ModelState.AddModelError("Password", "Sai tài khoản hoặc mật khẩu.");
            }
			ViewData["LoginModel"] = request;
			return View(request);
        }

        [HttpGet]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Google");
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities
                .FirstOrDefault()?.Claims.Select(claim =>
                    new
                    {
                        claim.Type,
                        claim.Value
                    });

            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;

            var user = await _authService.GetOrCreateAccountByEmail(email);

            var appClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(appClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                 new AuthenticationProperties
                 {
                     IsPersistent = true,
                     ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                 });


            if (user.Role.Equals(UserRole.customer.ToString()))
            {
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
            else if (user.Role.Equals(UserRole.manager.ToString()))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Manager" });
            }

            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
			ViewBag.ActiveTab = "register";
			if (ModelState.IsValid)
            {
                try
                {
                    var userModel = new UserModel
                    {
                        Username = model.Username,
                        Password = model.Password,
                        Email = model.Email
                    };
                    var user = await _authService.Register(userModel);

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
							IsPersistent = true,
							ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
						});
					return RedirectToAction("Index", "Home", new { area = "Customer" });
				}
				catch (UserAlreadyExistsByUsernameException ex)
				{
					ModelState.AddModelError("Username", ex.Message);
				}
				catch (UserAlreadyExistsByEmailException ex)
				{
					ModelState.AddModelError("Email", ex.Message);
				}
				catch (Exception ex)
                {
					ModelState.AddModelError("", ex.Message);
				}	
			}
			ViewData["RegisterModel"] = model;
			return View("Login");
		}

        public IActionResult ForgetPassword()
        {
			ViewBag.ActiveTab = "forgetPassword";
			return View("Login");
		}

        [HttpPost]
		public async Task<IActionResult> ForgetPassword(ForgetPasswordRequest request)
		{
			ViewBag.ActiveTab = "forgetPassword";
			ViewData["ForgetPasswordModel"] = request;
			var user = await _userSerivce.GetUserByEmail(request.Email);

			if (user == null)
			{
                ModelState.AddModelError("Email", "Không tìm thấy người dùng với email bạn cung cấp");
				return View("Login");
			}

            await _authService.SendPasswordResetLink(request.Email);

			ViewBag.Notification = "Gửi yêu cầu thành công";
			ViewBag.NotificationFull = "Gửi yêu cầu thành công, vui lòng kiểm tra email của bạn.";
			return View("Login");
		}

		public async Task<IActionResult> ResetPassword(string token, string email)
		{
			var tokenEntry = await _resetPasswordService.FindOneAsync(email, token);

			if (tokenEntry == null)
			{
				ModelState.AddModelError("", "Link không hợp lệ hoặc đã hết hạn.");
				return View();
			}
			var model = new ResetPasswordRequest { Token = token, Email = email, NewPassword = string.Empty, ConfirmPassword = string.Empty };
			return View(model);
		}

        [HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
		{
			if (!ModelState.IsValid)
				return View(request);

			var tokenEntry = await _resetPasswordService.FindOneAsync(request.Email, request.Token);

			if (tokenEntry == null)
			{
				ModelState.AddModelError("NewPassword", "Link không hợp lệ hoặc đã hết hạn.");
				return View(request);
			}

			var user = await _userSerivce.GetUserByEmail(request.Email);

			if (user == null)
			{
				ModelState.AddModelError("NewPassword", "Không tìm thấy người dùng với email: " + request.Email);
				return View(request);
			}

			user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

			await _userSerivce.Update(user);

			await _resetPasswordService.Delete(tokenEntry);

            TempData["Notification"] = "Đổi mật khẩu thành công, vui lòng đăng nhập với mật khẩu bạn mới nhập.";

            return RedirectToAction("Login");
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
