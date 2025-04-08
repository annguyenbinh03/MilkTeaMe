using MilkTeaMe.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.BusinessObjects
{
    public class MilkTeaModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public decimal PriceSizeS { get; set; }
        public decimal PriceSizeM { get; set; }
        public decimal PriceSizeL { get; set; }

        public Product? ToProduct()
        {
            if (this == null) return null;

            return new Product
            {
                Id = Id,
                Name = Name,
                Description = Description,
                ImageUrl = ImageUrl,
                Status = Status,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt
            };
        }
    }
}
