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
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerRepository customerRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerModel>>> GetCustomers()
        {
            try
            {
                var customers = await _customerRepository.GetCustomersAsync();
                var mappedCustomers = _mapper.Map<List<CustomerModel>>(customers);
                return Ok(mappedCustomers);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
        }

        [HttpGet("{customerId}")]
        public async Task<ActionResult<CustomerModel>> GetCustomer(int customerId)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerAsync(customerId);
                var mappedCustomer = _mapper.Map<CustomerModel>(customer);
                return Ok(mappedCustomer);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
        }

        [HttpGet("{customerId}/orders")]
        public async Task<ActionResult<List<OrderModel>>> GetCustomerOrders(int customerId)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerAsync(customerId);
                if (customer == null) return BadRequest("Customer does not exist");
                if (!customer.Orders.Any()) return BadRequest("No orders found!");
                var mappedOrders = _mapper.Map<OrderModel>(customer.Orders);
                return Ok(mappedOrders);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
        }

        [HttpGet("{customerId}/cart")]
        public async Task<ActionResult<ShoppingCartModel>> GetCustomerShoppingCart(int customerId)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerAsync(customerId);
                if (customer == null) return BadRequest("Customer does not exist");
                var mappedCart = _mapper.Map<ShoppingCartModel>(customer.ShoppingCart);
                return Ok(mappedCart);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CustomerModel>> AddCustomer([FromBody] CustomerModel customerModel)
        {
            try
            {
                var location = _linkGenerator.GetPathByAction("GetCustomer", "Customers", new { customerId = customerModel.CustomerId });
                if (string.IsNullOrEmpty(location))
                {
                    return BadRequest("Could not use current customer id");
                }

                var customer = _mapper.Map<Customer>(customerModel);
                _customerRepository.AddCustomer(customer);
                if (await _customerRepository.SaveChangesAsync())
                {
                    return Created(location, _mapper.Map<CustomerModel>(customer));
                }

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Opps!");
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
        }

        [HttpPut("{customerId}")]
        public async Task<ActionResult<CustomerModel>> UpdateCustomer(int customerId, [FromBody] CustomerModel newCustomerModel)
        {
            try
            {
                var oldCustomer = await _customerRepository.GetCustomerAsync(customerId);
                if (oldCustomer == null)
                {
                    return NotFound();
                }
                _mapper.Map(newCustomerModel, oldCustomer);
                if (await _customerRepository.SaveChangesAsync())
                {
                    return _mapper.Map<CustomerModel>(oldCustomer);
                }
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Opps!");
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Opps!");
            }
        }

        [HttpDelete("{customerId}")]
        public async Task<ActionResult<ProductModel>> DeleteCustomer(int customerId)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerAsync(customerId);
                if (customer == null)
                {
                    return NotFound();
                }
                _customerRepository.DeleteCustomer(customer);
                if (await _customerRepository.SaveChangesAsync())
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

        [HttpPost("{customerId}/cart")]
        public async Task<ActionResult<ShoppingCartModel>> CreateCustomerCart(int customerId)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerAsync(customerId);
                if (customer == null) return BadRequest("Customer does not exist");

                _customerRepository.CreateNewShoppingCart(customer);
                var cartModel = _mapper.Map<ShoppingCartModel>(customer.ShoppingCart);
                
                if(await _customerRepository.SaveChangesAsync())
                {
                    return Ok(_mapper.Map<ShoppingCart>(cartModel));
                }
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Oops!");
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
        }
    }
}
