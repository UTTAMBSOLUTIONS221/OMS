using DBL.Enums;
using DBL.Models;
using DBL.Repositories;
using DBL.Services;
using Moq;

namespace DBLTests
{
    public class DiscountServiceTests
    {
        private readonly Mock<IDiscountRuleRepository> _ruleRepositoryMock;
        private readonly DiscountService _discountService;

        public DiscountServiceTests()
        {
            _ruleRepositoryMock = new Mock<IDiscountRuleRepository>();

            // Default rules (simulating database fetch)
            var rules = new Dictionary<CustomerSegment, DiscountRule>
            {
                [CustomerSegment.Corporate] = new DiscountRule { BaseDiscount = 0.10m, LoyaltyBonus = 0m, LoyaltyThreshold = 0 },
                [CustomerSegment.Individual] = new DiscountRule { BaseDiscount = 0.07m, LoyaltyBonus = 0.02m, LoyaltyThreshold = 10 },
                [CustomerSegment.VIP] = new DiscountRule { BaseDiscount = 0.15m, LoyaltyBonus = 0.05m, LoyaltyThreshold = 5 },
                [CustomerSegment.Wholesale] = new DiscountRule { BaseDiscount = 0.20m, LoyaltyBonus = 0.01m, LoyaltyThreshold = 20 }
            };

            _ruleRepositoryMock.Setup(repo => repo.GetDiscountRules())
                               .ReturnsAsync(rules);

            _discountService = new DiscountService(_ruleRepositoryMock.Object);
        }

        private List<Orders> GenerateOrderHistory(int count)
        {
            var orders = new List<Orders>();
            for (int i = 1; i <= count; i++)
            {
                orders.Add(new Orders
                {
                    OrderId = i,
                    CreatedAt = DateTime.UtcNow.AddDays(-i),
                    TotalAmount = 100 + i
                });
            }
            return orders;
        }

        [Fact]
        public async Task ThrowExceptionWhenOrderIsNull()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => _discountService.CalculateDiscount(null));
            Assert.Contains("Order or Customer must not be null", ex.Message);
        }

        [Fact]
        public async Task ThrowExceptionWhenCustomerIsNull()
        {
            var order = new Orders { TotalAmount = 150m, Customer = null };

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => _discountService.CalculateDiscount(order));
            Assert.Contains("Order or Customer must not be null", ex.Message);
        }

        [Fact]
        public async Task ThrowExceptionWhenSegmentIsNotRegistered()
        {
            // Setup: Empty rule repository
            _ruleRepositoryMock.Setup(repo => repo.GetDiscountRules()).ReturnsAsync(new Dictionary<CustomerSegment, DiscountRule>());

            var order = new Orders
            {
                TotalAmount = 100m,
                Customer = new Customers
                {
                    Segment = CustomerSegment.Individual,
                    OrderHistory = new List<Orders>()
                }
            };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _discountService.CalculateDiscount(order));
            Assert.Contains("No discount rule configured", ex.Message);
        }

        [Theory]
        [InlineData(CustomerSegment.Corporate, 500, 0, 50)]     // 10% discount
        [InlineData(CustomerSegment.Individual, 200, 5, 14)]    // 7% only
        [InlineData(CustomerSegment.Individual, 300, 11, 27)]   // 7% + 2%
        [InlineData(CustomerSegment.Individual, 100, 0, 7)]     // base 7%
        [InlineData(CustomerSegment.Individual, 100, 20, 9)]    // 9% cap
        [InlineData(CustomerSegment.VIP, 100, 10, 20)]          // 15% + 5%
        [InlineData(CustomerSegment.Wholesale, 100, 25, 21)]    // 20% + 1%
        public async Task ShouldCalculateCorrectDiscount(
            CustomerSegment segment, decimal totalAmount, int orderHistoryCount, decimal expectedDiscount)
        {
            var customer = new Customers
            {
                Segment = segment,
                OrderHistory = GenerateOrderHistory(orderHistoryCount)
            };

            var order = new Orders
            {
                TotalAmount = totalAmount,
                Customer = customer
            };

            var discount = await _discountService.CalculateDiscount(order);

            Assert.Equal(expectedDiscount, discount);
        }

        [Fact]
        public async Task ShouldNotExceedTotalAmountEvenWithHighDiscount()
        {
            var order = new Orders
            {
                TotalAmount = 0.1m,
                Customer = new Customers
                {
                    Segment = CustomerSegment.VIP,
                    OrderHistory = GenerateOrderHistory(100)
                }
            };

            var discount = await _discountService.CalculateDiscount(order);

            Assert.True(discount <= order.TotalAmount);
            Assert.Equal(0.02m, discount); // 20% of 0.1 = 0.02
        }
    }
}
