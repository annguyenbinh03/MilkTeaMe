using MilkTeaMe.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<object> GetSalesData();
        Task<decimal> GetTotalRevenueByDay();
        Task<decimal> GetTotalRevenueByMonth();
        Task<decimal> GetTotalRevenueByYear();
        Task<IEnumerable<Product>> GetHighLightProduct();
    }
}
