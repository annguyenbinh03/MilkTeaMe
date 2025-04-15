using System.ComponentModel.DataAnnotations;

namespace MilkTeaMe.Web.Models.Requests
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Tên tài khoản không được để trống.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên tài khoản phải từ 3 đến 50 ký tự.")]
        [Display(Name = "Tên tài khoản")]
        public string Username { get; set; } = "";  

        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [StringLength(255, ErrorMessage = "Email không được vượt quá 255 ký tự.")]
        [Display(Name = "Địa chỉ Email")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 100 ký tự.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống.")]
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; } = "";

        [Display(Name = "Tôi đồng ý với các điều khoản")]
        [MustBeTrue(ErrorMessage = "Bạn phải đồng ý với các điều khoản để đăng ký.")]
        public bool AcceptTerms { get; set; } = false;
    }
    class MustBeTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            return value is bool b && b;
        }
    }
}
