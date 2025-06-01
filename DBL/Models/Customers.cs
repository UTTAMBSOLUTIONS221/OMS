using DBL.Enums;
using System.ComponentModel.DataAnnotations;

namespace DBL.Models
{
    public class Customers
    {
        [Key]
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public CustomerSegment Segment { get; set; }
        public ICollection<Orders> OrderHistory { get; set; } = new List<Orders>();
    }
}
