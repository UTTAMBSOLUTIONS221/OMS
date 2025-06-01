using DBL.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBL.Models
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customers Customer { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? FulfilledAt { get; set; }
        [Precision(18, 2)]
        public decimal TotalAmount { get; set; }
        [Precision(18, 2)]
        public decimal DiscountedAmount { get; set; }
        public OrderStatus Status { get; set; }
    }
}
