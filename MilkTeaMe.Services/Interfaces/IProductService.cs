using MilkTeaMe.Repositories.Models;
using MilkTeaMe.Services.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product?> GetProduct(int id);
        Task<(IEnumerable<Product>, int)> GetMilkTeas(string? search, int? page = null, int? pageSize = null);
		Task<MilkTeaModel?> GetMilkTea(int id);
		Task CreateMilkTea(MilkTeaModel model);
		Task UpdateMilkTea(MilkTeaModel model);
		Task<(IEnumerable<Product>, int)> GetToppings(string? search, int? page = null, int? pageSize = null);
		Task<Product?> GetTopping(int id);
		Task CreateTopping(ToppingModel model);
		Task UpdateTopping(ToppingModel model);
		Task<(IEnumerable<ComboModel>, int)> GetCombos(string? search, int? page = null, int? pageSize = null);
		Task<Product?> GetCombo(int id);
		Task CreateCombo(ComboModel model);
		Task Delete(int id);
		Task Active(int id);
	}
}
