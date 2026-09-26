namespace Ordering.Domain.ValueObjects;

public record Payment
{
    public string? CardName { get; } = string.Empty;
    public string CardNumber { get; } = string.Empty;
    public string Expiration { get; } = string.Empty;
    public string CVV { get; } = string.Empty;
    public string PaymentMethod { get; } = string.Empty;
}
