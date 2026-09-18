using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountDbContext(DbContextOptions<DiscountDbContext> options)
    : DbContext(options)
{
    public DbSet<Coupon> Coupons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>().HasData(
            new Coupon
            {
                Id = 1,
                ProductName = "Samsung Galaxy Z Fold",
                Description = "Discounted on Samsung newly launch z fold smartphone",
                Amount = 10000
            },
            new Coupon
            {
                Id = 2,
                ProductName = "Samsung Galaxy Z Fold Tri",
                Description = "Discounted on Samsung newly launch z fold tri smartphone",
                Amount = 15000
            },
            new Coupon
            {
                Id = 3,
                ProductName = "Iphone Duo",
                Description = "Discounted on apple newly launch iphone duo smartphone",
                Amount = 12000
            },
            new Coupon
            {
                Id = 4,
                ProductName = "Iphone 18 pro max",
                Description = "Discounted on apple newly launch iphone 18 pro max smartphone",
                Amount = 6000
            }
        );
    }
}
