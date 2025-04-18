using Microsoft.AspNetCore.Identity;
using MilkTeaMe.Repositories.Enums;
using MilkTeaMe.Repositories.Models;
using MilkTeaMe.Repositories.UnitOfWork;
using MilkTeaMe.Services.BusinessObjects;
using MilkTeaMe.Services.Exceptions;
using MilkTeaMe.Services.Interfaces;
using MilkTeaMe.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.Implementations
{
    public class AuthService : IAuthService
    {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IEmailService _emailService;
		public AuthService(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<User> GetOrCreateAccountByEmail(string email)
        {
           User? user = await _unitOfWork.UserRepository.FindOneAsync(filter: filter => filter.Email == email);

            if(user == null)
            {
                User newUser = new User
                {
                    Username = email,
                    Role = UserRole.customer.ToString(),
                    Email = email,
                    Status = UserStatus.active.ToString(),
                    CreatedAt = TimeZoneUtil.GetCurrentTime(),
                    UpdatedAt = TimeZoneUtil.GetCurrentTime(),
                };

                await _unitOfWork.UserRepository.InsertAsync(newUser);
                await _unitOfWork.SaveChangesAsync();
                return newUser;
            }
            return user;
        }

		public async Task<User?> Login(string username, string password)
		{
			var user = await _unitOfWork.UserRepository
				.FindOneAsync(u => u.Username == username);

			if (user == null)
				return null;

			return BCrypt.Net.BCrypt.Verify(password, user.Password) ? user : null;
		}

		public async Task<User> Register(UserModel model)
		{

            User? existingUser = await _unitOfWork.UserRepository.FindOneAsync(filter: u => u.Username == model.Username);

            if (existingUser != null) {
                throw new UserAlreadyExistsByUsernameException();
			}

			existingUser = await _unitOfWork.UserRepository.FindOneAsync(filter: u => u.Email == model.Email);

			if (existingUser != null)
			{
				throw new UserAlreadyExistsByEmailException();
			}

			User user = new User
            {
                Username = model.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = UserRole.customer.ToString(),
                Email = model.Email,
                Status = UserStatus.active.ToString(),
                CreatedAt = TimeZoneUtil.GetCurrentTime(),
                UpdatedAt = TimeZoneUtil.GetCurrentTime(),
            };
            await _unitOfWork.UserRepository.InsertAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return user;
		}

		public async Task SendPasswordResetLink(string email)
		{
			var user = await _unitOfWork.UserRepository.FindOneAsync(filter: u => u.Email == email) ?? throw new UserNotFound();

			var token = Guid.NewGuid().ToString();
			var expiredAt = TimeZoneUtil.GetCurrentTime().AddMinutes(15);
			await _unitOfWork.PasswordResetTokenRepository.InsertAsync(new PasswordResetToken
			{
				Email = email,
				Token = token,
				ExpiredAt = expiredAt
			});
			await _unitOfWork.SaveChangesAsync();

			var resetLink = $"https://localhost:7119/Auth/ResetPassword?token={token}&email={email}";

			var form = @"
                <!DOCTYPE html>
                <html lang=""vi"">
                <head>
                    <meta charset=""UTF-8"">
                    <style>
                        body {
                            font-family: ""Segoe UI"", sans-serif;
                            background-color: #f3f3f3;
                            margin: 0;
                            padding: 0;
                        }

                        .email-container {
                            background-color: #ffffff;
                            max-width: 600px;
                            margin: 40px auto;
                            border-radius: 8px;
                            overflow: hidden;
                            box-shadow: 0 0 10px rgba(0,0,0,0.1);
                            border: 1px solid black;
                        }

                        .email-header {
                            background-color: #0078D4;
                            color: white;
                            padding: 20px;
                            text-align: center;
                        }

                        .email-body {
                            padding: 30px;
                            color: #333333;
                        }

                        .email-body h2 {
                            color: #0078D4;
                        }

                        .btn {
                            display: inline-block;
                            margin-top: 20px;
                            padding: 12px 20px;
                            background-color: #0078D4;
                            color: white !important;
                            text-decoration: none;
                            border-radius: 6px;
                        }

                        .email-footer {
                            font-size: 12px;
                            color: #888888;
                            padding: 20px;
                            text-align: center;
                        }
                    </style>
                </head>
                <body>
                    <div class=""email-container"">
                        <div class=""email-header"">
                            <h1>Đặt lại mật khẩu</h1>
                        </div>
                        <div class=""email-body"">
                            <h2>Xin chào,</h2>
                            <p>Bạn đã yêu cầu đặt lại mật khẩu. Click vào nút bên dưới để thay đổi mật khẩu của bạn:</p>

                            <a href=""{{resetLink}}"" class=""btn"">Đổi mật khẩu</a>

                            <p>Nếu bạn không yêu cầu điều này, bạn có thể bỏ qua email này.</p>
                            <p>Liên kết sẽ hết hạn sau 15 phút.</p>
                        </div>
                        <div class=""email-footer"">
                            © 2025 MilkTeaMe. Tất cả các quyền được bảo lưu.
                        </div>
                    </div>
                </body>
                </html>";


			var message = form.Replace("{{resetLink}}", resetLink);
			await _emailService.SendEmailAsync(email, "Yêu cầu thay đổi mật khẩu", message);
		}
	}
}
