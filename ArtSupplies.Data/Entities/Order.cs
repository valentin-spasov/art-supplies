using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSupplies.Data.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateShipped { get; set; }
        public IEnumerable<CartItem> OrderItems { get; set; }
        public OrderStatus Status { get; set; }
    }
}
