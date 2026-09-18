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
        logger.LogInformation("Executing get discount for request {@Request}", request);
        logger.LogInformation($"Fetching coupon for product name {request.ProductName} from database");
        var coupon = await dbContext.Coupons
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ProductName.ToLower()
                .Equals(request.ProductName.ToLower()));

        if (coupon is null)
        {
            logger.LogInformation($"No coupon found for product name {request.ProductName} in database");
            logger.LogInformation("Executed get discount with response {@Response}", NoDiscountModel);
            return NoDiscountModel;
        }

        logger.LogInformation($"Coupon found successfully for product name {coupon.ProductName}");
        var response = coupon.ToModel();
        logger.LogInformation("Executed get discount with response {@Response}", response);
        return response;
    }

    public override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        return base.CreateDiscount(request, context);
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
