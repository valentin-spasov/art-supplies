using ArtSupplies.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSupplies.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ArtSuppliesDbContext _context;

        public CustomerRepository(ArtSuppliesDbContext context)
        {
            _context = context;
        }
        public void AddCustomer(Customer c)
        {
            if(c == null)
            {
                throw new ArgumentNullException(nameof(c));
            }
            _context.Add(c);
        }

        public void DeleteCustomer(Customer c)
        {
            if (c == null)
            {
                throw new ArgumentNullException(nameof(c));
            }
            _context.Remove(c);
            // Removing the address. Maybe this shouldn't be here
            _context.Address.Remove(c.Address);
        }

        public async Task<Customer> GetCustomerAsync(int customerId)
        {
            return await _context.Customers
                .Include(c => c.ShoppingCart)
                .Include(c => c.Address)
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await _context.Customers
                .Include(c => c.ShoppingCart)
                .Include(c => c.Address)
                .Include(c => c.Orders)
                .ToListAsync();
        }

        public void CreateNewShoppingCart(Customer c)
        {
            var cart = new ShoppingCart{ CartItems = null, DateCreated = DateTime.Now, Total = 0};
            _context.Add(cart);
            c.ShoppingCart = cart;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
