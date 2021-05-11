using ArtSupplies.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSupplies.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ArtSuppliesDbContext _context;

        public OrderRepository(ArtSuppliesDbContext context)
        {
            _context = context;
        }
        public async Task<Order> GetOrder(int orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _context.Orders.Include(o => o.OrderItems).ToListAsync();
        }

        public async Task<Order> PayOrder(int orderId)
        {
            var order = await GetOrder(orderId);
            // Payment
            order.Status = OrderStatus.Payed;
            return order;
        }

        public void RemoveOrder(Order order)
        {
            if(order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            _context.Remove(order);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
