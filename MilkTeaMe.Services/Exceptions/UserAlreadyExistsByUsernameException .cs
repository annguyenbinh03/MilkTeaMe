using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.Exceptions
{
	public class UserAlreadyExistsByUsernameException : Exception
	{
		private const string DefaultMessage = "Đã tồn tại tài khoản khác sử dụng tên tài khoản này, xin dùng tên tài khoản khác.";
		public UserAlreadyExistsByUsernameException() : base(DefaultMessage) { }

		public UserAlreadyExistsByUsernameException(string message) : base(message) { }
	}
}
