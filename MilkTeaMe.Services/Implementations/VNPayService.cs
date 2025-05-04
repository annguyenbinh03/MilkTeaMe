using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using MilkTeaMe.Repositories.Enums;
using MilkTeaMe.Repositories.Models;
using MilkTeaMe.Repositories.UnitOfWork;
using MilkTeaMe.Services.BusinessObjects;
using MilkTeaMe.Services.Interfaces;
using MilkTeaMe.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MilkTeaMe.Services.Implementations
{
	public class VNPayService : IVNPayService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly VNPaySettings _vnPaySettings;

		public VNPayService(IOptions<VNPaySettings> vnPaySettings, IUnitOfWork unitOfWork)
		{
			_vnPaySettings = vnPaySettings.Value;
			_unitOfWork = unitOfWork;
		}

		public async Task<string> Charge(int orderId)
		{

			var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
			if (order == null)
			{
				throw new Exception("Order not found");
			}
			if (order.Status.Equals("completed") || order.Status.Equals("cancelled"))
			{
				throw new Exception("Order has been completed or cancelled");
			}
			decimal totalAmount = order.TotalPrice;
			//VNPay don't accept payment with the value < 0
			if (totalAmount <= 0)
			{
				throw new Exception("Total amount must be greater than 0");
			}
			string paymentUrl = CreatePaymentUrl(totalAmount, orderId);

			return paymentUrl;
		}

		public async Task<string> ConfirmPayment(HttpRequest request)
		{
			if (!request.QueryString.HasValue)
			{
				throw new Exception("VNPAY Querry string not found");
			}

			var queryString = request.QueryString.Value;
			var json = HttpUtility.ParseQueryString(queryString);

			string txnRef = json["vnp_TxnRef"].ToString();
			int orderId = Convert.ToInt32(json["vnp_OrderInfo"]);
			long vnpayTranId = Convert.ToInt64(json["vnp_TransactionNo"]);
			string vnp_ResponseCode = json["vnp_ResponseCode"].ToString();
			string vnp_SecureHash = json["vnp_SecureHash"].ToString();
			string stringAmount = json["vnp_Amount"].ToString();
			int pos = queryString.IndexOf("&vnp_SecureHash");

			bool checkSignature = ValidateSignature(queryString.Substring(1, pos - 1), vnp_SecureHash, _vnPaySettings.HashSecret);
			if (!checkSignature || _vnPaySettings.TmnCode != json["vnp_TmnCode"].ToString())
			{
				throw new Exception("Invalid signature or incorrect merchant code.");
			}

			decimal amount;
			bool isParseSucess = Decimal.TryParse(stringAmount, out amount);

			if (!isParseSucess)
			{
				throw new Exception("Parse giá trị đơn hàng thất bại.");
			}

			amount /= 100;

            var payment = new Payment
            {
                OrderId = orderId,
                TransactionCode = vnpayTranId.ToString(),
                CreatedAt = TimeZoneUtil.GetCurrentTime(),
                Amount = amount,
                PaymentMethodId = (int)PaymentMethodId.vnpay,
                Status = vnp_ResponseCode == "00" ? PaymentStatus.completed.ToString() : PaymentStatus.failed.ToString(),
            };

            await _unitOfWork.PaymentRepository.InsertAsync(payment);

            if (vnp_ResponseCode == "00")
            {
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId)
                            ?? throw new Exception("Order not found with id:" + orderId);
                order.Status = OrderStatus.completed.ToString();
            }

            await _unitOfWork.SaveChangesAsync();

            return _vnPaySettings.RedirectUrl + "/" + payment.Id;
		}

		public string CreatePaymentUrl(decimal amount, int orderId)
		{
			string hostName = System.Net.Dns.GetHostName();
			string serverIPAddress = System.Net.Dns.GetHostAddresses(hostName).GetValue(0).ToString();
			string infor = "Thanh toan cho orderId: " + orderId.ToString();
			string tnxRef = TimeZoneUtil.GetCurrentTime().ToString("ddHHmmssyyyy");
			tnxRef = tnxRef + orderId.ToString();

			string vnp_Amount = ((int)amount).ToString() + "00";

			VNPayHelper pay = new VNPayHelper();
			pay.AddRequestData("vnp_Version", "2.1.0");
			pay.AddRequestData("vnp_Command", "pay");
			pay.AddRequestData("vnp_TmnCode", _vnPaySettings.TmnCode);
			pay.AddRequestData("vnp_Amount", vnp_Amount);
			pay.AddRequestData("vnp_BankCode", "");
			pay.AddRequestData("vnp_CreateDate", TimeZoneUtil.GetCurrentTime().ToString("yyyyMMddHHmmss"));
			pay.AddRequestData("vnp_CurrCode", "VND");
			pay.AddRequestData("vnp_IpAddr", serverIPAddress);
			pay.AddRequestData("vnp_Locale", "vn");
			pay.AddRequestData("vnp_OrderInfo", orderId.ToString());
			pay.AddRequestData("vnp_OrderType", "other");
			pay.AddRequestData("vnp_ReturnUrl", _vnPaySettings.ReturnUrl);
			pay.AddRequestData("vnp_TxnRef", tnxRef);

			return pay.CreateRequestUrl(_vnPaySettings.VnpayUrl, _vnPaySettings.HashSecret);
		}

		private bool ValidateSignature(string rspraw, string inputHash, string secretKey)
		{
			string myChecksum = VNPayHelper.HmacSHA512(secretKey, rspraw);
			return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
		}
	}
}
