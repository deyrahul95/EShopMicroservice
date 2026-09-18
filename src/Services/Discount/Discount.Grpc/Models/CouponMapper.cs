namespace Discount.Grpc.Models;

public static class CouponMapper
{
    public static CouponModel ToModel(this Coupon coupon) => new()
    {
        Id = coupon.Id,
        ProductName = coupon.ProductName,
        Amount = coupon.Amount,
        Description = coupon.Description
    };
}
