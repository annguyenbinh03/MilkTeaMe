using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.Exceptions
{
	public class UserNotFound : Exception
	{
		private const string DefaultMessage = "Không tìm thấy người dùng.";
		public UserNotFound() : base(DefaultMessage) { }

		public UserNotFound(string message) : base(message) { }
	}
}
