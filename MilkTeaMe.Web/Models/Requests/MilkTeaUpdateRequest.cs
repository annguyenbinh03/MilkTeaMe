using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MilkTeaMe.Web.Models.Requests
{
	public class MilkTeaUpdateRequest
	{
		public int Id { get; set; }
		[DisplayName("Tên sản phẩm")]
		[Required(ErrorMessage = "Phải nhập tên sản phẩm")]
		[StringLength(255, MinimumLength = 3, ErrorMessage = "Tên sản phẩm phải dài hơn 3 kí tự và bé hơn 255 ký tự")]
		public required string Name { get; set; }
		[DisplayName("Mô tả của sản phẩm")]
		[Required(ErrorMessage = "Phải nhập mô tả sản phẩm")]
		public required string Description { get; set; }
		[DisplayName("Giá cho Size S")]
		[Required(ErrorMessage = "Phải nhập giá sản phẩm")]
		[DataType(DataType.Currency)]
		[Range(0.01, 1000000, ErrorMessage = "Giá sản phẩm phải nằm trong khoảng từ 0.01 đến 1000000.")]
		public required decimal PriceSizeS { get; set; }
		[DisplayName("Giá cho Size M")]
		[Required(ErrorMessage = "Phải nhập giá sản phẩm")]
		[DataType(DataType.Currency)]
		[Range(0.01, 1000000, ErrorMessage = "Giá sản phẩm phải nằm trong khoảng từ 0.01 đến 1000000.")]
		public required decimal PriceSizeM { get; set; }
		[DisplayName("Giá cho Size L")]
		[Required(ErrorMessage = "Phải nhập giá sản phẩm")]
		[DataType(DataType.Currency)]
		[Range(0.01, 1000000, ErrorMessage = "Giá sản phẩm phải nằm trong khoảng từ 0.01 đến 1000000.")]
		public required decimal PriceSizeL { get; set; }
		[DisplayName("Ảnh của sản phẩm")]
		public IFormFile? Image { get; set; }
		public required string ImageUrl { get; set; }
	}
}
