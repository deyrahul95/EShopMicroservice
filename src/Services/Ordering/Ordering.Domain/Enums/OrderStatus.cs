namespace Ordering.Domain.Enums;

public static class OrderStatus
{
    public const string Draft = "Draft";
    public const string Pending = "Pending";
    public const string Completed = "Completed";
    public const string Cancelled = "Cancelled";

    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase)
    {
        Draft,
        Pending,
        Completed,
        Cancelled
    };

    public static bool IsValid(string? value)
    {
        return value is not null && Allowed.Contains(value);
    }
}
