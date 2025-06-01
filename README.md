 DiscountService Implementation

This project contains a DiscountService used to apply dynamic, rule-based discounts to customer orders. Discounts are based on customer segments and order history, with all discount rules fetched from a database.

Project Structure

- DBL.Models: Contains the models such as Orders, Customers, and DiscountRule.
- DBL.Enums: Contains enumerations like CustomerSegment and OrderStatus.
- DBL.Services: Houses the DiscountService and OrderStatusService class.
- DBL.Repositories: Defines interfaces like IDiscountRuleRepository for accessing discount rules and IOrderRepository for managing order.
- DBLTests: xUnit test project containing unit tests for the DiscountService with tests for checking if Order is null, customer is null etc.
- API: this project contains the API endpoints for the DiscountService.

Features

- Discounts vary by customer segment.
- Loyalty bonuses are applied based on order history count.
- Loyalty Threashold is the number of counts to make one loyal customer.
- All rules are stored in and retrieved from the database.
- Discounts are capped to never exceed the order total.

Assumptions

1. Customer Segments:
   - Corporate, Individual, VIP, Wholesale, etc. are predefined in the CustomerSegment enum.

2. Discount Rules:
   - Each segment has a rule defined in the database via a DiscountRule entity.
   - Rules are loaded from the database via an IDiscountRuleRepository abstraction.

3. Order History:
   - Loyalty bonuses are calculated based on the count of a customer’s past orders.
   - It is the caller's responsibility to ensure Customer.OrderHistory is loaded when calculating discounts.

4. EF Core Usage:
   - Ensure related data like Customer.OrderHistory is included via .Include() during query operations.

Usage
Inject the Service

builder.Services.AddScoped<IDiscountRuleRepository, DiscountRuleRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<DiscountService>();
builder.Services.AddScoped<OrderStatusService>();
