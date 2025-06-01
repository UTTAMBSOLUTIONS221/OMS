using DBL.Enums;
using DBL.Models;

namespace DBL.Services
{
    public class OrderStatusService
    {
        // Define allowed state transitions
        private static readonly Dictionary<OrderStatus, OrderStatus[]> ValidTransitions = new()
        {
            { OrderStatus.Created, new[] { OrderStatus.Paid, OrderStatus.Cancelled } },
            { OrderStatus.Paid, new[] { OrderStatus.Shipped, OrderStatus.Cancelled } },
            { OrderStatus.Shipped, new[] { OrderStatus.Delivered } },
            { OrderStatus.Delivered, Array.Empty<OrderStatus>() },
            { OrderStatus.Cancelled, Array.Empty<OrderStatus>() }
        };
        // Transition an order to the next status, validating the transition
        // Automatically sets the fulfillment timestamp if transitioning to Delivered

        public void Transition(Orders order, OrderStatus nextStatus)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            if (!ValidTransitions.TryGetValue(order.Status, out var allowedStatuses) || !allowedStatuses.Contains(nextStatus))
            {
                throw new InvalidOperationException($"Invalid transition from '{order.Status}' to '{nextStatus}'.");
            }

            order.Status = nextStatus;

            if (nextStatus == OrderStatus.Delivered)
            {
                order.FulfilledAt = DateTime.UtcNow;
            }
        }
    }
}
