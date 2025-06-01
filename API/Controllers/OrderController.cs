using DBL.Entities;
using DBL.Enums;
using DBL.Models;
using DBL.Repositories;
using DBL.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly DiscountService _discountService;
        private readonly OrderStatusService _orderStatusService;

        public OrderController(IOrderRepository orderRepository, ICustomerRepository customerRepository, DiscountService discountService, OrderStatusService orderStatusService)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _discountService = discountService;
            _orderStatusService = orderStatusService;
        }

        // POST: api/orders
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var customer = await _customerRepository.GetCustomerById(dto.CustomerId);
            if (customer == null)
            {
                return NotFound($"Customer with ID {dto.CustomerId} not found.");
            }

            var order = new Orders
            {
                CustomerId = customer.CustomerId,
                CreatedAt = DateTime.UtcNow,
                TotalAmount = dto.TotalAmount,
                Status = OrderStatus.Created,
                // Apply discount based on segment + order history
                DiscountedAmount = dto.TotalAmount - await _discountService.CalculateDiscount(
                    new Orders
                    {
                        Customer = customer,
                        TotalAmount = dto.TotalAmount
                    })
            };
            var created = await _orderRepository.Create(order);
            var fullOrder = await _orderRepository.GetOrderById(created.OrderId);
            return Ok(MapToOrderDetailDto(fullOrder));
        }
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] OrderStatus nextStatus)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }


            var fromStatus = order.Status;

            try
            {
                _orderStatusService.Transition(order, nextStatus);
                await _orderRepository.Update(order);

                // Log transition
                await _orderRepository.LogOrderStatusChange(order.OrderId, fromStatus, nextStatus);

                var updatedOrder = await _orderRepository.GetOrderById(id);
                return Ok(MapToOrderDetailDto(updatedOrder));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("analytics")]
        public async Task<IActionResult> GetAnalytics()
        {
            var analytics = await _orderRepository.GetOrderAnalytics();
            return Ok(analytics);
        }
        private OrderDetailDto MapToOrderDetailDto(Orders order)
        {
            return new OrderDetailDto
            {
                OrderId = order.OrderId,
                TotalAmount = order.TotalAmount,
                DiscountedAmount = order.DiscountedAmount,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                Customer = new CustomerDataDto
                {
                    CustomerId = order.Customer.CustomerId,
                    FirstName = order.Customer.FirstName,
                    LastName = order.Customer.LastName,
                    Segment = order.Customer.Segment
                }
            };
        }
    }
}
