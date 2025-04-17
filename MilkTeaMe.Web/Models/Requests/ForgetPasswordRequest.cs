using System.ComponentModel.DataAnnotations;

namespace MilkTeaMe.Web.Models.Requests
{
	public class ForgetPasswordRequest
	{
		[Required(ErrorMessage = "Email không được để trống.")]
		[EmailAddress(ErrorMessage = "Email không hợp lệ.")]
		[StringLength(255, ErrorMessage = "Email không được vượt quá 255 ký tự.")]
		[Display(Name = "Địa chỉ Email")]
		public string Email { get; set; } = "";
	}
}
