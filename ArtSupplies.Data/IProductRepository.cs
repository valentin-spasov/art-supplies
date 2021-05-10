using ArtSupplies.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSupplies.Data
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(ProductCategory category);
        Task<Product> GetProductAsync(int productId);
        void AddProduct(Product p);
        void DeleteProduct(Product p);
        Task<bool> SaveChangesAsync();
    }
}
