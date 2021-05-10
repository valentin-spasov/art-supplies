using ArtSupplies.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSupplies.Data
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetCustomersAsync();
        Task<Customer> GetCustomerAsync(int customerId);
        void AddCustomer(Customer c);
        void DeleteCustomer(Customer c);
        void CreateNewShoppingCart(Customer c);
        Task<bool> SaveChangesAsync();
    }
}
