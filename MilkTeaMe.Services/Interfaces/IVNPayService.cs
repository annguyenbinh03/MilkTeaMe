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
		/// <summary>
		/// Create a url to redirect vnpay for payemnt
		/// </summary>
		/// <param name="orderId">Id of order</param>
		/// <returns></returns>
		Task<string> Charge(int orderId);
		/// <summary>
		/// VNPay will call this function to handle payment status
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<string> ConfirmPayment(HttpRequest request);
	}
}
