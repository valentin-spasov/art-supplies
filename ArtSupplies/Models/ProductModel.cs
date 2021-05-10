using ArtSupplies.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArtSupplies.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        public string ImageFileName { get; set; }
        public ProductCategory Category { get; set; }
    }
}
