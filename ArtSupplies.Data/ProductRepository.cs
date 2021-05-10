using ArtSupplies.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSupplies.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly ArtSuppliesDbContext _context;

        public ProductRepository(ArtSuppliesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(ProductCategory category)
        {
            if(category != ProductCategory.None)
            {
                return await _context.Products.Where(p => p.Category == category).OrderByDescending(p => p.ProductName).ToListAsync();
            }
            return await _context.Products.OrderByDescending(p => p.ProductName).ToListAsync();
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public void AddProduct(Product p)
        {
            if(p == null)
            {
                throw new ArgumentNullException(nameof(p));
            }
            _context.Add(p);
        }

        public void DeleteProduct(Product p)
        {
            if (p == null)
            {
                throw new ArgumentNullException(nameof(p));
            }
            _context.Remove(p);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
