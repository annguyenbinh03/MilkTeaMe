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

namespace MilkTeaMe.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(IEnumerable<Product>, int)> GetMilkTeas(string? search, int? page = null, int? pageSize = null)
        {
            var (products, totalItems) = await _unitOfWork.ProductRepository.GetMilkTeas(search, page, pageSize);
            return (products, totalItems);
        }

		public async Task<MilkTeaModel?> GetMilkTea(int id)
		{
			var product = await _unitOfWork.ProductRepository.GetMilkTea(id);
			if (product == null)
				return null;
			MilkTeaModel model = new MilkTeaModel
			{
				Id = id,
				Name = product.Name,
				Description = product.Description,
				ImageUrl = product.ImageUrl,
				Status = product.Status,
				PriceSizeS = product.ProductSizes.ToList()[0].Price,
				PriceSizeM = product.ProductSizes.ToList()[1].Price,
				PriceSizeL = product.ProductSizes.ToList()[2].Price,
			};

			return model;
		}

		public async Task CreateMilkTea(MilkTeaModel model)
		{
			Product? product = model.ToProduct();

			if (product == null)
			{
				throw new Exception("Can not parse product");
			}
			try
			{
				product.CategoryId = (int)ProductCategoryEnum.MilkTea;
				product.Status = ProductStatus.active.ToString();
				product.CreatedAt = TimeZoneUtil.GetCurrentTime();
				product.UpdatedAt = TimeZoneUtil.GetCurrentTime();

				await _unitOfWork.ProductRepository.InsertAsync(product);

				var productSizeS = new ProductSize
				{
					Product = product,
					SizeId = 1,
					Price = (decimal)model.PriceSizeS
				};
				await _unitOfWork.ProductSizeRepository.InsertAsync(productSizeS);

				var productSizeM = new ProductSize
				{
					Product = product,
					SizeId = 2,
					Price = (decimal)model.PriceSizeM
				};
				await _unitOfWork.ProductSizeRepository.InsertAsync(productSizeM);

				var productSizeL = new ProductSize
				{
					Product = product,
					SizeId = 3,
					Price = (decimal)model.PriceSizeL
				};
				await _unitOfWork.ProductSizeRepository.InsertAsync(productSizeL);

				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task UpdateMilkTea(MilkTeaModel model)
		{
			Product? product = await _unitOfWork.ProductRepository.GetByIdAsync(model.Id);

			if (product == null) return;

			product.UpdatedAt = TimeZoneUtil.GetCurrentTime();

			if (product.Name != model.Name)
			{
				product.Name = model.Name;
			}

			if (product.Description != model.Description)
			{
				product.Description = model.Description;
			}

			if (product.ImageUrl != model.ImageUrl && !string.IsNullOrEmpty(model.ImageUrl))
			{
				product.ImageUrl = model.ImageUrl;
			}

			if (product == null)
			{
				throw new Exception("Can not parse product");
			}
			try
			{
				_unitOfWork.ProductRepository.Update(product);

				var productSizeS = await _unitOfWork.ProductSizeRepository.FindOneAsync(filter: ps => ps.ProductId == model.Id && ps.SizeId == (int)ProductSizeEnum.S);
				if (productSizeS != null)
				{
					productSizeS.Price = (decimal)model.PriceSizeS;
					_unitOfWork.ProductSizeRepository.Update(productSizeS);
				}

				var productSizeM = await _unitOfWork.ProductSizeRepository.FindOneAsync(filter: ps => ps.ProductId == model.Id && ps.SizeId == (int)ProductSizeEnum.M);
				if (productSizeM != null)
				{
					productSizeM.Price = (decimal)model.PriceSizeM;
					_unitOfWork.ProductSizeRepository.Update(productSizeM);
				}


				var productSizeL = await _unitOfWork.ProductSizeRepository.FindOneAsync(filter: ps => ps.ProductId == model.Id && ps.SizeId == (int)ProductSizeEnum.L);
				if (productSizeL != null)
				{
					productSizeL.Price = (decimal)model.PriceSizeL;
					_unitOfWork.ProductSizeRepository.Update(productSizeL);
				}
				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}

		public async Task<(IEnumerable<Product>, int)> GetToppings(string? search, int? page = null, int? pageSize = null)
		{
			var (products, totalItems) = await _unitOfWork.ProductRepository.GetToppings(search, page, pageSize);
			return (products.ToList(), totalItems);
		}

		public async Task<Product?> GetTopping(int id)
		{
			var product = await _unitOfWork.ProductRepository.GetTopping(id);
			if (product == null)
				return null;

			return product;
		}

		public async Task CreateTopping(ToppingModel model)
		{
			Product? product = model.ToProduct();

			if (product == null)
			{
				throw new Exception("Can not parse product");
			}
			try
			{
				product.Status = ProductStatus.active.ToString();
				product.CreatedAt = TimeZoneUtil.GetCurrentTime();
				product.UpdatedAt = TimeZoneUtil.GetCurrentTime();

				await _unitOfWork.ProductRepository.InsertAsync(product);
				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task UpdateTopping(ToppingModel model)
		{
			Product? product = await _unitOfWork.ProductRepository.GetByIdAsync(model.Id);

			if (product == null) return;

			product.UpdatedAt = TimeZoneUtil.GetCurrentTime();

			if (product.Name != model.Name)
			{
				product.Name = model.Name;
			}

			if (product.Description != model.Description)
			{
				product.Description = model.Description;
			}

			if (product.ImageUrl != model.ImageUrl && !string.IsNullOrEmpty(model.ImageUrl))
			{
				product.ImageUrl = model.ImageUrl;
			}

			if (product.Price != model.Price)
			{
				product.Price = model.Price;
			}

			if (product == null)
			{
				throw new Exception("Can not parse product");
			}
			try
			{
				_unitOfWork.ProductRepository.Update(product);

				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<(IEnumerable<ComboModel>, int)> GetCombos(string? search, int? page = null, int? pageSize = null)
		{
			var (products, totalItems) = await _unitOfWork.ProductRepository.GetCombos(search, page, pageSize);
			List<ComboModel> comboModels = new List<ComboModel>();
			foreach (var combo in products)
			{
				var comboModel = new ComboModel
				{
					Id = combo.Id,
					Name = combo.Name,
					Description = combo.Description,
					Price = combo.Price,
					ImageUrl = combo.ImageUrl,
					SoldCount = combo.SoldCount,
					Status = combo.Status,
					UpdatedAt = combo.UpdatedAt,
					Products = new List<ProductInCombo>()
				};

				foreach (var productCombo in combo.ProductComboCombos)
				{
					var product = await _unitOfWork.ProductRepository.GetByIdAsync(productCombo.ProductId);
					if (product != null)
					{
						var productComboModel = new ProductInCombo
						{
							Id = product.Id,
							Name = product.Name,
							Quantity = productCombo.Quantity,
							ImageUrl = product.ImageUrl,
							Price = product.Price,
						};

						if (productCombo.ProductSizeId.HasValue)
						{
							var productSize = await _unitOfWork.ProductSizeRepository.GetByIdAsync(productCombo.ProductSizeId, includes: ps => ps.Size);

							if(productSize != null)
							{
								productComboModel.Size = productSize.Size.Name.ToString();
								comboModel.Products.Add(productComboModel);
							}
						}
					}
				}

				comboModels.Add(comboModel);
			}
			return (comboModels, totalItems);
		}

		public async Task<Product?> GetCombo(int id)
		{
			var product = await _unitOfWork.ProductRepository.GetCombo(id);
			if (product == null)
				return null;

			return product;
		}

		public async Task CreateCombo(ComboModel model)
		{
			Product? product = new Product();
			product.Id = model.Id;
			product.Name = model.Name;
			product.Description = model.Description;
			product.Price = model.Price;
			product.ImageUrl = model.ImageUrl;
			product.CategoryId = 2;
			product.Status = ProductStatus.active.ToString();
			product.CreatedAt = TimeZoneUtil.GetCurrentTime();
			product.UpdatedAt = TimeZoneUtil.GetCurrentTime();

			if (product == null)
			{
				throw new Exception("Can not parse product");
			}
			try
			{
				await _unitOfWork.ProductRepository.InsertAsync(product);

				foreach (ProductInCombo item in model.Products)
				{
					ProductCombo combo = new ProductCombo();
					combo.ProductId = item.Id;
					combo.Quantity = item.Quantity;
					combo.Combo = product;

					if (!string.IsNullOrEmpty(item.Size))
					{
						ProductSizeEnum sizeEnum = (ProductSizeEnum)Enum.Parse(typeof(ProductSizeEnum), item.Size);
						int sizeValue = (int)sizeEnum;
						ProductSize? productSize = await _unitOfWork.ProductSizeRepository.FindOneAsync(filter: ps => ps.ProductId == item.Id && ps.SizeId == sizeValue);
						if (productSize == null) {
							throw new Exception($"Can not find product with size {combo.ProductId} - {item.Size}");
						}
						combo.ProductSizeId = productSize.Id;
					}
					else
					{
						combo.ProductSizeId = null;
						combo.ProductSize = null;
					}
					await _unitOfWork.ProductComboRepository.InsertAsync(combo);
				}
				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task Delete(int id)
		{
			Product? product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

			if (product == null) return;
			product.Status = ProductStatus.deleted.ToString();
			product.UpdatedAt = TimeZoneUtil.GetCurrentTime();

			_unitOfWork.ProductRepository.Update(product);

			await _unitOfWork.SaveChangesAsync();
		}

		public async Task Active(int id)
		{
			Product? product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

			if (product == null) return;
			product.Status = ProductStatus.active.ToString();
			product.UpdatedAt = TimeZoneUtil.GetCurrentTime();

			_unitOfWork.ProductRepository.Update(product);

			await _unitOfWork.SaveChangesAsync();
		}

        public async Task<Product?> GetProduct(int id)
        {
            return await _unitOfWork.ProductRepository.GetByIdAsync(id);
        }
    }
}
