using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSupplies.Data.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageFileName { get; set; }
        public ProductCategory Category { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<ShoppingCart> Carts { get; set; }
    }
}
