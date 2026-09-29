namespace Ordering.Domain.ValueObjects;

public record OrderName
{
    private const int MinLength = 5;
    private const int MaxLength = 100;

    public string Value { get; }

    private OrderName(string value) => Value = value;

    public static OrderName Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfLessThan(value.Length, MinLength);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Length, MaxLength);

        return new OrderName(value);
    }
}
