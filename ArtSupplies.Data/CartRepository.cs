using ArtSupplies.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSupplies.Data
{
    public class CartRepository : ICartRepository
    {
        private readonly ArtSuppliesDbContext _context;

        public CartRepository(ArtSuppliesDbContext context)
        {
            _context = context;
        }

        public async Task<CartItem> AddItemShoppingCartAsync(int productId, int cartId)
        {
            var item = await GetCartItemAsync(productId, cartId);
            var cart = await GetShoppingCartAsync(cartId);
            var product = await _context.Products.FindAsync(productId);

            if(item != null)
            {
                item.Quantity++;
                cart.Total += product.Price;
                return item;
            }

            item = new CartItem { ProductId = productId, ShoppingCartId = cartId, Quantity = 1 };
            await _context.CartItems.AddAsync(item);
            cart.Total += product.Price;
            return item;
        }

        public async Task<CartItem> GetCartItemAsync(int productId, int cartId)
        {
            return await _context.CartItems.FindAsync(productId, cartId);
        }

        public async Task<ShoppingCart> GetShoppingCartAsync(int cartId)
        {
            return await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.ShoppingCartId == cartId);
        }

        public async Task<IEnumerable<ShoppingCart>> GetShoppingCartsAsync()
        {
            return await _context.Carts.Include(c => c.CartItems).ToListAsync();
        }

        public async Task<CartItem> RemoveItemShoppingCart(CartItem cartItem)
        {
            if(cartItem == null)
            {
                throw new ArgumentNullException(nameof(cartItem));
            }
            var cart = await GetShoppingCartAsync(cartItem.ShoppingCartId);
            var product = await _context.Products.FindAsync(cartItem.ProductId);

            if(cartItem.Quantity == 1)
            {
                cartItem.Quantity = 0;
                _context.Remove(cartItem);
                cart.Total -= product.Price;
                return cartItem;
            }

            cartItem.Quantity--;
            cart.Total -= product.Price;
            return cartItem;
        }

        public async Task<Order> CreateOrder(ShoppingCart cart)
        {
            if(cart == null)
            {
                throw new ArgumentNullException(nameof(cart));
            }

            var orderItems = await _context.CartItems.Where(ci => ci.ShoppingCartId == cart.ShoppingCartId).ToListAsync();
            var order = new Order() { DateCreated = DateTime.Now, DateShipped = DateTime.MinValue, Status = OrderStatus.New, OrderItems = orderItems };
            _context.Orders.Add(order);
            return order;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
