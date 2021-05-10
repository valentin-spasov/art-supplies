using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSupplies.Data.Entities
{
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        public DateTime DateCreated { get; set; }
        public IEnumerable<Product> CartItems { get; set; }
        public double Total { get; set; }
    }
}
