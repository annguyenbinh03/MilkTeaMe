using MilkTeaMe.Repositories.Models;
using MilkTeaMe.Repositories.UnitOfWork;
using MilkTeaMe.Services.BusinessObjects;
using MilkTeaMe.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.Implementations
{
	public class PaymentService : IPaymentService
	{
		private readonly IUnitOfWork _unitOfWork;

		public PaymentService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<PaymentModel> GetPaymentInfo(int paymentId)
		{
			Payment payment = await _unitOfWork.PaymentRepository.GetByIdAsync(paymentId, p => p.PaymentMethod) ?? throw new Exception("Payment not found with id: " + paymentId );

			PaymentModel model = new PaymentModel
			{
				Id = paymentId,
				OrderId = payment.OrderId,
				Amount = payment.Amount,
				TransactionCode = payment.TransactionCode,
				Status = payment.Status,
				CreatedAt = payment.CreatedAt,
				MethodName = payment.PaymentMethod.Name,
			};

			return model;
		}
	}
}
