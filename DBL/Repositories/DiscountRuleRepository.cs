using DBL.Data;
using DBL.Enums;
using DBL.Models;
using Microsoft.EntityFrameworkCore;

namespace DBL.Repositories
{
    public class DiscountRuleRepository : IDiscountRuleRepository
    {
        private readonly ApplicationDbContext _context;

        public DiscountRuleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        // This method retrieves all discount rules from the database and returns them as a dictionary.
        public async Task<Dictionary<CustomerSegment, DiscountRule>> GetDiscountRules()
        {
            var rules = await _context.DiscountRules.ToListAsync();

            return rules.ToDictionary(
                r => r.Segment,
                r => new DiscountRule
                {
                    BaseDiscount = r.BaseDiscount,
                    LoyaltyBonus = r.LoyaltyBonus,
                    LoyaltyThreshold = r.LoyaltyThreshold
                });
        }
    }
}
