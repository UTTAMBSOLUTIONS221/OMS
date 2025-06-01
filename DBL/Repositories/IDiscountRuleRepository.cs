using DBL.Enums;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IDiscountRuleRepository
    {
        Task<Dictionary<CustomerSegment, DiscountRule>> GetDiscountRules();
    }
}
