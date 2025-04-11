namespace MilkTeaMe.Web.Models.Requests
{
	public class CartItemRequest
	{
		public int ProductId { get; set; }
		public string Size { get; set; } = string.Empty;
		public int Quantity { get; set; }
		public List<int> Toppings { get; set; } = new();
	}
}
