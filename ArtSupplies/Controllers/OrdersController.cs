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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public OrdersController(IOrderRepository orderRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderModel>>> GetOrders()
        {
            try
            {
                var orders = await _orderRepository.GetOrders();
                var mappedOrders = _mapper.Map<List<OrderModel>>(orders);
                return Ok(mappedOrders);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderModel>> GetOrder(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetOrder(orderId);
                var mappedOrder = _mapper.Map<OrderModel>(order);
                return Ok(mappedOrder);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
        }

        [HttpPost("{orderId}")]
        public async Task<ActionResult<OrderModel>> PayOrder(int orderId)
        {
            try
            {
                var order = await _orderRepository.PayOrder(orderId);
                if (order == null) { BadRequest("No such order"); }

                var mappedOrder = _mapper.Map<OrderModel>(order);
                return Ok(mappedOrder);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
        }

        [HttpDelete("{orderId}")]
        public async Task<ActionResult<OrderModel>> DeleteOrder(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetOrder(orderId);
                if (order == null) { BadRequest("No such order"); }
                _orderRepository.RemoveOrder(order);

                return _mapper.Map<OrderModel>(order);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }
        }
    }
}
