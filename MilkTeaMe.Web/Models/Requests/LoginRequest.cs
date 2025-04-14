using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MilkTeaMe.Web.Models.Requests
{
    public class LoginRequest
    {
        [DisplayName("Tên tài khoản")]
        [Required(ErrorMessage = "Tên tài khoản không được để trống")]
        public string Username { get; set; } = null!;
        [DisplayName("Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; } = false;
    }
}
