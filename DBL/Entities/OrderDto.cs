using System.ComponentModel.DataAnnotations;

namespace DBL.Entities
{
    public class OrderDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [Range(100, double.MaxValue, ErrorMessage = "Total amount must be greater than 100.")]
        public decimal TotalAmount { get; set; }
    }
}
