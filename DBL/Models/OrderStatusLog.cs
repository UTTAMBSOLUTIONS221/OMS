using DBL.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBL.Models
{
    public class OrderStatusLog
    {
        [Key]
        public int LogId { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Orders Order { get; set; }
        public OrderStatus FromStatus { get; set; }
        public OrderStatus ToStatus { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}
