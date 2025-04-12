using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.BusinessObjects
{
	public class VNPaySettings
	{
		public required string VnpayUrl { get; set; }
		public required string ReturnUrl { get; set; }
		public required string TmnCode { get; set; }
		public required string HashSecret { get; set; }
		public required string RedirectUrl { get; set; }
	}
}
