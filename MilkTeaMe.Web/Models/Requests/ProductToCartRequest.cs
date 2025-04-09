namespace MilkTeaMe.Web.Models.Requests
{
    public class ProductToCartRequest
    {
        public required int ProductId { get; set; }
        public string? Size { get; set; }
        public required int Quantity { get; set; }
        public List<int> Toppings { get; set; } = new List<int>();
    }
}
