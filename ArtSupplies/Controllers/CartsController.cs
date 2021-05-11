using ArtSupplies.Data;
using ArtSupplies.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtSupplies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public CartsController(ICartRepository cartRepository, IProductRepository productRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ShoppingCartModel>>> GetShoppingCarts()
        {
            try
            {
                var carts = await _cartRepository.GetShoppingCartsAsync();
                var mappedCarts = _mapper.Map<List<ShoppingCartModel>>(carts);
                return Ok(mappedCarts);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
        }

        [HttpGet("{cartId}")]
        public async Task<ActionResult<ShoppingCartModel>> GetShoppingCart(int cartId)
        {
            try
            {
                var cart = await _cartRepository.GetShoppingCartAsync(cartId);
                var mappedCart = _mapper.Map<ShoppingCartModel>(cart);
                return Ok(mappedCart);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
        }

        [HttpPost("{cartId}/items/{productId}")]
        public async Task<ActionResult<CartItemModel>> AddItemToCart(int cartId, int productId)
        {
            try
            {
                var location = _linkGenerator.GetPathByAction("GetCartItem", "Carts", new { cartId, productId });

                if (string.IsNullOrEmpty(location))
                {
                    return BadRequest("Could not use current cart id and product id");
                }

                var item = await _cartRepository.AddItemShoppingCartAsync(productId, cartId);

                if (await _cartRepository.SaveChangesAsync())
                {
                    return Created(location, _mapper.Map<CartItemModel>(item));
                }

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Opps!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
        }

        [HttpGet("{cartId}/items/{productId}")]
        public async Task<ActionResult<CartItemModel>> GetCartItem(int cartId, int productId)
        {
            try
            {
                var item = await _cartRepository.GetCartItemAsync(productId, cartId);
                if (item == null) return BadRequest("Item does not exist");
                return _mapper.Map<CartItemModel>(item);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
        }

        [HttpDelete("{cartId}/items/{productId}")]
        public async Task<ActionResult<CartItemModel>> DeleteCartItem(int cartId, int productId)
        {
            try
            {
                var item = await _cartRepository.GetCartItemAsync(productId, cartId);
                if (item == null) return BadRequest("Item does not exist");
                await _cartRepository.RemoveItemShoppingCart(item);
                if (await _cartRepository.SaveChangesAsync())
                {
                    return _mapper.Map<CartItemModel>(item);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
        }

        [HttpPost("{cartId}/order")]
        public async Task<ActionResult<CartItemModel>> CreateOrder(int cartId)
        {
            try
            {
                var cart = await _cartRepository.GetShoppingCartAsync(cartId);
                if(cart.CartItems == null) { return BadRequest("Cannot checkout without items"); }

                var item = await _cartRepository.CreateOrder(cart);

                if (item == null) return BadRequest("Item does not exist");

                return _mapper.Map<CartItemModel>(item);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
        }
    }
}
