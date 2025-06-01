namespace DBL.Models
{
    public class OrderAnalyticsDto
    {
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public TimeSpan? AverageFulfillmentTime { get; set; }
        public int DeliveredOrders { get; set; }
        public int CancelledOrders { get; set; }
    }
}
