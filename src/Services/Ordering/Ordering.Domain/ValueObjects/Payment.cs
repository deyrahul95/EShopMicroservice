using System.ComponentModel.DataAnnotations;
using Ordering.Domain.Exceptions;

namespace Ordering.Domain.ValueObjects;

public record Payment
{
    [MaxLength(50)]
    public string CardName { get; } = string.Empty;
    [MaxLength(24)]
    public string CardNumber { get; } = string.Empty;
    [MaxLength(10)]
    public string Expiration { get; } = string.Empty;
    [MaxLength(3)]
    public string CVV { get; } = string.Empty;
    [Required]
    public string PaymentMethod { get; } = string.Empty;

    protected Payment() { }

    private Payment(
        string cardName,
        string cardNumber,
        string expiration,
        string cvv,
        string paymentMethod)
    {
        CardName = cardName;
        CardNumber = cardNumber;
        Expiration = expiration;
        CVV = cvv;
        PaymentMethod = paymentMethod;
    }

    public static Payment Of(
        string cardName,
        string cardNumber,
        string expiration,
        string cvv,
        string paymentMethod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(paymentMethod);

        if (Enums.PaymentMethod.IsValid(paymentMethod) == false)
        {
            throw new DomainException($"Invalid payment method.");
        }

        if (IsCardPayment(paymentMethod: paymentMethod))
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cardName);
            ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);
            ArgumentException.ThrowIfNullOrWhiteSpace(cvv);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);
            ArgumentException.ThrowIfNullOrWhiteSpace(expiration);
        }

        return new Payment(
            cardName: cardName,
            cardNumber: cardNumber,
            expiration: expiration,
            cvv: cvv,
            paymentMethod: paymentMethod);
    }

    private static bool IsCardPayment(string paymentMethod)
    {
        return string.Equals(
               paymentMethod,
               Enums.PaymentMethod.CreditCard,
               StringComparison.OrdinalIgnoreCase)
            || string.Equals(
               paymentMethod,
               Enums.PaymentMethod.DebitCard,
               StringComparison.OrdinalIgnoreCase);
    }
}
