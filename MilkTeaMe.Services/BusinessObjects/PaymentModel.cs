using MilkTeaMe.Repositories.Models;

namespace MilkTeaMe.Services.BusinessObjects
{
	public class PaymentModel
	{
		public int Id { get; set; }

		public int OrderId { get; set; }

		public decimal Amount { get; set; }

		public string TransactionCode { get; set; } = string.Empty;

		public string Status { get; set; } = string.Empty;

		public DateTime? CreatedAt { get; set; }

		public string MethodName { get; set; } = string.Empty;
	}
}
