using DBL.Enums;

namespace DBL.Models
{
    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public CustomerDataDto Customer { get; set; }
    }

    public class CustomerDataDto
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public CustomerSegment Segment { get; set; }
    }
}
