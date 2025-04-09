namespace MilkTeaMe.Web.Models.Responses
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string? Size { get; set; }
        public int Quantity { get; set; }
        public List<int> ToppingIds { get; set; }
    }
}
