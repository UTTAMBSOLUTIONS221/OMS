using DBL.Enums;
using System.ComponentModel.DataAnnotations;

namespace DBL.Models
{
    public class DiscountRule
    {
        [Key]
        public int DiscountRuleId { get; set; }
        public CustomerSegment Segment { get; set; }
        public decimal BaseDiscount { get; set; }
        public decimal LoyaltyBonus { get; set; }
        public int LoyaltyThreshold { get; set; }
    }
}
