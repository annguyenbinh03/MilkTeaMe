using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.BusinessObjects
{
    public class CartItemModel
    {
        public int ProductId { get; set; }
        public string? Size { get; set; }
        public int Quantity { get; set; }
        public List<int> Toppings { get; set; } = new();
    }
}
