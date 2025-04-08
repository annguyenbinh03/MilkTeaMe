using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MilkTeaMe.Web.Models.Requests
{
	public class ComboCreationRequest
	{
		[DisplayName("Tên sản phẩm")]
		[Required(ErrorMessage = "Tên combo là bắt buộc.")]
		[StringLength(255, MinimumLength = 3, ErrorMessage = "Tên sản phẩm phải dài hơn 3 kí tự và bé hơn 255 ký tự")]
		public string Name { get; set; } = string.Empty;
		[DisplayName("Mô tả của sản phẩm")]
		[Required(ErrorMessage = "Phải nhập mô tả sản phẩm")]
		public string Description { get; set; } = string.Empty;
		[DisplayName("Giá cho combo")]
		[Required(ErrorMessage = "Phải nhập giá sản phẩm")]
		[DataType(DataType.Currency)]
		[Range(0.01, 10000000, ErrorMessage = "Giá sản phẩm phải nằm trong khoảng từ 0.01 đến 10000000.")]
		public decimal? Price { get; set; }
		[DisplayName("Ảnh của sản phẩm")]
		[Required(ErrorMessage = "Phải gửi lên ảnh sản phẩm")]
		public required IFormFile Image { get; set; }
		public List<ProductInComboCreationRequest> Products { get; set; } = new List<ProductInComboCreationRequest>();
	}
	public class ProductInComboCreationRequest
	{
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public int? SizeId { get; set; }
		public string? SizeName { get; set; }
		public string? Name { get; set; }
		public string? ImageUrl { get; set; }
		public int? Price { get; set; }
	}
}
