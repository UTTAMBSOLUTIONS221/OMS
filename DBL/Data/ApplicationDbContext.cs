using DBL.Models;
using Microsoft.EntityFrameworkCore;

namespace DBL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<DiscountRule> DiscountRules { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderStatusLog> OrderStatusLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DiscountRule>().HasKey(o => o.DiscountRuleId);
            modelBuilder.Entity<DiscountRule>().Property(o => o.BaseDiscount).HasPrecision(18, 2);
            modelBuilder.Entity<DiscountRule>().Property(o => o.LoyaltyBonus).HasPrecision(18, 2);
            modelBuilder.Entity<DiscountRule>().Property(o => o.LoyaltyThreshold).HasPrecision(18, 2);
            modelBuilder.Entity<Orders>().HasKey(o => o.OrderId);
            modelBuilder.Entity<Orders>().Property(o => o.TotalAmount).HasPrecision(18, 2);
            modelBuilder.Entity<Orders>().Property(o => o.DiscountedAmount).HasPrecision(18, 2);
            modelBuilder.Entity<Customers>().HasKey(o => o.CustomerId);
            modelBuilder.Entity<Orders>().HasOne(o => o.Customer).WithMany(c => c.OrderHistory).HasForeignKey(o => o.CustomerId);
        }
    }
}
