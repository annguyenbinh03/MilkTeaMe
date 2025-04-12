using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MilkTeaMe.Services.Interfaces
{
	public interface IVNPayService
	{
		Task<string> Charge(int orderId);
		Task<string> ConfirmPayment(HttpRequest request);
	}
}
