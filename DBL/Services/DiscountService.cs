using DBL.Enums;
using DBL.Models;
using DBL.Repositories;

namespace DBL.Services
{
    public class DiscountService
    {
        private readonly IDiscountRuleRepository _ruleRepository;

        public DiscountService(IDiscountRuleRepository ruleRepository)
        {
            _ruleRepository = ruleRepository;
        }
        private readonly Dictionary<CustomerSegment, DiscountRule> _rules;

        public DiscountService(Dictionary<CustomerSegment, DiscountRule> rules)
        {
            _rules = rules;
        }

        public async Task<decimal> CalculateDiscount(Orders order)
        {
            if (order == null || order.Customer == null)
            {
                throw new ArgumentNullException("Order or Customer must not be null.");
            }

            var segment = order.Customer.Segment;
            var orderCount = order.Customer.OrderHistory?.Count ?? 0;

            var rules = await _ruleRepository.GetDiscountRules();

            if (!rules.TryGetValue(segment, out var rule))
            {
                throw new InvalidOperationException($"No discount rule configured for segment {segment}");
            }

            decimal discount = order.TotalAmount * rule.BaseDiscount;

            if (orderCount > rule.LoyaltyThreshold)
            {
                discount += order.TotalAmount * rule.LoyaltyBonus;
            }

            return Math.Min(discount, order.TotalAmount);
        }
    }
}
