using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.Exceptions
{
	public class UserAlreadyExistsByEmailException : Exception
	{
		private const string DefaultMessage = "Đã tồn tại tài khoản khác sử dụng email này, xin dùng email khác.";
		public UserAlreadyExistsByEmailException() : base(DefaultMessage) { }

		public UserAlreadyExistsByEmailException(string message) : base(message) { }
	}
}
