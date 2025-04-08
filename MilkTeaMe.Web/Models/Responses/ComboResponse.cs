namespace MilkTeaMe.Web.Models.Responses
{
	public class ComboResponse
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public decimal? Price { get; set; } 
		public string ImageUrl { get; set; } = string.Empty;
		public List<ProductSizeViewModel> Sizes { get; set; }  
		public bool IsMilkTea { get; set; } 
	}
	public class ProductSizeViewModel
	{
		public int SizeId { get; set; }
		public string SizeName { get; set; } = string.Empty;
		public decimal Price { get; set; }
	}
}
