using MilkTeaMe.Repositories.Models;
using MilkTeaMe.Repositories.UnitOfWork;
using MilkTeaMe.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> GetHighLightProduct()
        {
            var (products, totalItem) = await _unitOfWork.ProductRepository.GetAsync(orderBy: p => p.OrderByDescending(product => product.SoldCount));
            return products;
        }

        public async Task<object> GetSalesData()
        {
            DateTime today = DateTime.Today;
            List<DateTime> labels = Enumerable.Range(0, 7)
                .Select(i => today.AddDays(-i))
                .ToList();
            Dictionary<DateTime, decimal> salesMap = labels.ToDictionary(date => date, _ => 0m);

            List<Order> orders = await GetOrdersInLast7Days();

            foreach (var order in orders)
            {
                DateTime orderDate = ((DateTime)order.CreatedAt).Date;
                if (salesMap.ContainsKey(orderDate))
                {
                    salesMap[orderDate] += order.TotalPrice;
                }
            }

            return new
            {
                labels = labels.Select(d => d.ToString("yyyy-MM-dd")).Reverse(),
                data = labels.Select(d => salesMap[d]).Reverse()
            };
        }

        public async Task<decimal> GetTotalRevenueByDay()
        {
            DateTime date = DateTime.Today;
            var (orders, totalItems) = await _unitOfWork.OrderRepository.GetAsync(o => ((DateTime)o.CreatedAt).Date == date);
            decimal totalRevenue = orders.ToList().Sum(o => o.TotalPrice);
            return totalRevenue;
        }

        public async Task<decimal> GetTotalRevenueByMonth()
        {
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endDate = startDate.AddMonths(1);

            var (orders, totalItems) = await _unitOfWork.OrderRepository.GetAsync(o => o.CreatedAt >= startDate && o.CreatedAt < endDate);
            decimal totalRevenue = orders.AsEnumerable().Sum(o => o.TotalPrice);
            return totalRevenue;
        }

        public async Task<decimal> GetTotalRevenueByYear()
        {
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endDate = startDate.AddYears(-1);

            var (orders, totalItems) = await _unitOfWork.OrderRepository.GetAsync(o => o.CreatedAt >= startDate && o.CreatedAt < endDate);
            decimal totalRevenue = orders.AsEnumerable().Sum(o => o.TotalPrice);
            return totalRevenue;
        }

        private async Task<List<Order>> GetOrdersInLast7Days()
        {
            DateTime today = DateTime.Today.AddDays(1);
            DateTime startDate = today.AddDays(-6); 

            var (orders, totalItems) = await _unitOfWork.OrderRepository
                .GetAsync(o => o.CreatedAt >= startDate && o.CreatedAt <= today);
            return orders.ToList();
        }
    }
}
