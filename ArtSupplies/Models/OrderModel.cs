using ArtSupplies.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtSupplies.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public DateTime DateShipped { get; set; }
        public IEnumerable<CartItem> OrderItems { get; set; }
        public OrderStatus Status { get; set; }
    }
}
