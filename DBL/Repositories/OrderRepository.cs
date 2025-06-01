using DBL.Data;
using DBL.Enums;
using DBL.Models;
using Microsoft.EntityFrameworkCore;

namespace DBL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        // This method creates a new order in the database.
        public async Task<Orders> Create(Orders order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }
        // This method retrieves an order by its ID, including the customer details.
        public async Task<Orders?> GetOrderById(int id)
        {
            return await _context.Orders.Include(o => o.Customer).FirstOrDefaultAsync(o => o.OrderId == id);
        }
        // This method updates an existing order in the database.
        public async Task Update(Orders order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
        // This method calculates various analytics related to orders, such as total orders, average order value, delivered orders, cancelled orders, and average fulfillment time.
        // It retrieves all orders from the database, filters them based on their status, and computes the required metrics.
        // The method returns an OrderAnalyticsDto object containing the calculated values.
        // Note: This method uses AsNoTracking to improve performance for read-only operations, as it does not need to track changes to the entities.
        public async Task<OrderAnalyticsDto> GetOrderAnalytics()
        {
            var orders = await _context.Orders.AsNoTracking().ToListAsync();

            var deliveredOrders = orders.Where(o => o.Status == OrderStatus.Delivered && o.FulfilledAt.HasValue).ToList();

            return new OrderAnalyticsDto
            {
                TotalOrders = orders.Count,
                AverageOrderValue = orders.Any() ? orders.Average(o => o.TotalAmount) : 0,
                DeliveredOrders = deliveredOrders.Count,
                CancelledOrders = orders.Count(o => o.Status == OrderStatus.Cancelled),
                AverageFulfillmentTime = deliveredOrders.Any() ? TimeSpan.FromSeconds(deliveredOrders.Average(o => (o.FulfilledAt.Value - o.CreatedAt).TotalSeconds)) : null
            };
        }
        // This method logs a change in order status, capturing the order ID, previous status, new status, and the time of change.
        public async Task LogOrderStatusChange(int orderId, OrderStatus fromStatus, OrderStatus toStatus)
        {
            var log = new OrderStatusLog
            {
                OrderId = orderId,
                FromStatus = fromStatus,
                ToStatus = toStatus,
                ChangedAt = DateTime.UtcNow
            };

            _context.OrderStatusLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
