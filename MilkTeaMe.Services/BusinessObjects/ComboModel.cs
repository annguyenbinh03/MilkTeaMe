using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.BusinessObjects
{
	public class ComboModel
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public decimal? Price { get; set; }

		public string ImageUrl { get; set; } = string.Empty;

		public int? SoldCount { get; set; }

		public string Status { get; set; } = string.Empty;

		public DateTime? UpdatedAt { get; set; }

		public List<ProductInCombo> Products { get; set; } = new List<ProductInCombo>();
	}
	public class ProductInCombo
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public int Quantity { get; set; }
		public string ImageUrl { get; set; } = string.Empty;
		public decimal? Price { get; set; }
		public string? Size { get; set; }
	}
}
