using ArtSupplies.Data;
using ArtSupplies.Data.Entities;
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
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _productRepository = productRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<ProductModel>>> GetProducts(ProductCategory category = ProductCategory.None)
        {
            try
            {
                var products = await _productRepository.GetAllProductsAsync(category);
                var mappedProducts = _mapper.Map<List<ProductModel>>(products);
                return Ok(mappedProducts);
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Opps!"); 
            }
        }
        [HttpGet("{productId}")]
        public async Task<ActionResult<ProductModel>> GetProduct(int productId)
        {
            try
            {
                var product = await _productRepository.GetProductAsync(productId);
                var mappedProduct = _mapper.Map<ProductModel>(product);
                return Ok(mappedProduct);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Opps!");
            }
        }
        [HttpPost]
        public async Task<ActionResult<ProductModel>> AddProduct([FromBody] ProductModel productModel)
        {
            try
            {
                var location = _linkGenerator.GetPathByAction("GetProduct", "Products", new { productId = productModel.ProductId });
                if(string.IsNullOrEmpty(location))
                {
                    return BadRequest("Could not use current product id");
                }

                var product = _mapper.Map<Product>(productModel);
                _productRepository.AddProduct(product);
                if (await _productRepository.SaveChangesAsync())
                {
                    return Created(location, _mapper.Map<ProductModel>(product));
                }

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Opps!");
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Opps!");
            }
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult<ProductModel>> UpdateProduct(int productId, [FromBody] ProductModel newProductModel)
        {
            try
            {
                var oldProduct = await _productRepository.GetProductAsync(productId);
                if(oldProduct == null)
                {
                    return NotFound();
                }
                _mapper.Map(newProductModel, oldProduct);
                if(await _productRepository.SaveChangesAsync())
                {
                    return _mapper.Map<ProductModel>(oldProduct);
                }
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Opps!");
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Opps!");
            }
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult<ProductModel>> DeleteProduct(int productId)
        {
            try
            {
                var product = await _productRepository.GetProductAsync(productId);
                if (product == null)
                {
                    return NotFound();
                }
                _productRepository.DeleteProduct(product);
                if (await _productRepository.SaveChangesAsync())
                {
                    return Ok("Deleted");
                }
                return BadRequest("Failed to delete the product");
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Opps!");
            }
        }
    }
}
