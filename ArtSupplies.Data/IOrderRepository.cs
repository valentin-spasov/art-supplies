using ArtSupplies.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSupplies.Data
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<Order> GetOrder(int orderId);
        Task<Order> PayOrder(int orderId);
        void RemoveOrder(Order order);
        Task<bool> SaveChangesAsync();
    }
}
