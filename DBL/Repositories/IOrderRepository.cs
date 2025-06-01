using DBL.Enums;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IOrderRepository
    {
        Task<Orders> Create(Orders order);
        Task<Orders> GetOrderById(int OrderId);
        Task Update(Orders order);
        Task<OrderAnalyticsDto> GetOrderAnalytics();
        Task LogOrderStatusChange(int orderId, OrderStatus fromStatus, OrderStatus toStatus);
    }
}
