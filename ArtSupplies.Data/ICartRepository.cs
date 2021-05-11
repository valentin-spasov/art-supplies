using ArtSupplies.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtSupplies.Data
{
    public interface ICartRepository
    {
        Task<IEnumerable<ShoppingCart>> GetShoppingCartsAsync();
        Task<ShoppingCart> GetShoppingCartAsync(int cartId);
        Task<CartItem> GetCartItemAsync(int productId, int cartId);
        Task<CartItem> AddItemShoppingCartAsync(int productId, int cartId);
        Task<CartItem> RemoveItemShoppingCart(CartItem cartItem);
        Task<Order> CreateOrder(ShoppingCart cart);
        Task<bool> SaveChangesAsync();
    }
}
