namespace Ordering.Domain.Enums;

public static class PaymentMethod
{
    public const string CreditCard = "CreditCard";
    public const string DebitCard = "DebitCard";
    public const string UPI = "UPI";
    public const string COD = "Cash On Delivery";

    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase)
    {
        CreditCard,
        DebitCard,
        UPI,
        COD
    };

    public static bool IsValid(string? value)
    {
        return value is not null && Allowed.Contains(value);
    }
}
