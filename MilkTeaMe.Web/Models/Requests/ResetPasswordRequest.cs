using System.ComponentModel.DataAnnotations;

namespace MilkTeaMe.Web.Models.Requests
{
	public class ResetPasswordRequest
	{
		[Required(ErrorMessage = "Email là bắt buộc.")]
		[EmailAddress(ErrorMessage = "Email không hợp lệ.")]
		[MaxLength(50, ErrorMessage = "Email tối đa 50 ký tự.")]
		public required string Email { get; set; }

		[Required(ErrorMessage = "Token là bắt buộc.")]
		public required string Token { get; set; }

		[Required(ErrorMessage = "Mật khẩu mới là bắt buộc.")]
		[MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
		public required string NewPassword { get; set; }

		[Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc.")]
		[Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu không khớp.")]
		public required string ConfirmPassword { get; set; }
	}
}