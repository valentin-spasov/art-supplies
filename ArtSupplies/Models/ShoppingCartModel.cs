using ArtSupplies.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtSupplies.Models
{
    public class ShoppingCartModel
    {
        public int ShoppingCartId { get; set; }
        public IEnumerable<ProductModel> CartItems { get; set; }
        public double Total { get; set; }
    }
}
