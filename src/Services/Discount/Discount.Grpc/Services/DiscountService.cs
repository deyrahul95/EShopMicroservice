using System.Text.Json;
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

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        logger.LogInformation("Executing update discount request {@Request}", request);

        var coupon = new Coupon
        {
            Id = request.Id,
            ProductName = request.ProductName,
            Description = request.Description,
            Amount = request.Amount
        };

        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation($"Discount updated successfully. Product name {coupon.ProductName}");

        var response = coupon.ToModel();
        logger.LogInformation("Executed update discount. Response {@Response}", response);
        return response;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        logger.LogInformation("Executing delete discount request {@Request}", request);
        logger.LogInformation($"Fetching discount for product name {request.ProductName} from database");
        var coupon = await dbContext.Coupons
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ProductName.ToLower()
                .Equals(request.ProductName.ToLower()));

        if (coupon is null)
        {
            logger.LogInformation($"No discount found for product name {request.ProductName} in database");
            throw new RpcException(new Status(
                StatusCode.NotFound,
                $"Discount with product name {request.ProductName} is not found in database."));
        }

        dbContext.Remove(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation(
            "Discount deleted successfully. Product name: {@ProductName}, Coupon: {@Coupon}",
            coupon.ProductName,
            JsonSerializer.Serialize(coupon));

        var response = new DeleteDiscountResponse { IsSuccess = true };
        logger.LogInformation("Executed delete discount. Response {@Response}", response);
        return response;
    }
}
