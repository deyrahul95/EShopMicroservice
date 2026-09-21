using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(
    DiscountDbContext dbContext,
    ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly CouponModel NoDiscountModel = new()
    {
        Id = 0000,
        ProductName = "No discount",
        Amount = 0,
        Description = "Product has no discount"
    };

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        logger.LogInformation("Executing get discount request {@Request}", request);
        logger.LogInformation($"Fetching discount for product name {request.ProductName} from database");
        var coupon = await dbContext.Coupons
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ProductName.ToLower()
                .Equals(request.ProductName.ToLower()));

        if (coupon is null)
        {
            logger.LogInformation($"No discount found for product name {request.ProductName} in database");
            logger.LogInformation("Executed get discount. Response {@Response}", NoDiscountModel);
            return NoDiscountModel;
        }

        logger.LogInformation($"Discount found successfully for product name {coupon.ProductName}");
        var response = coupon.ToModel();
        logger.LogInformation("Executed get discount. Response {@Response}", response);
        return response;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        logger.LogInformation("Executing create discount request {@Request}", request);

        var coupon = new Coupon
        {
            ProductName = request.ProductName,
            Description = request.Description,
            Amount = request.Amount
        };

        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation($"Discount added successfully. Product name {coupon.ProductName}");

        var response = coupon.ToModel();
        logger.LogInformation("Executed create discount. Response {@Response}", response);
        return response;
    }

    public override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        return base.UpdateDiscount(request, context);
    }

    public override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        return base.DeleteDiscount(request, context);
    }
}
