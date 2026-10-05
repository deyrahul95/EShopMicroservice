using Ordering.Application.Dtos;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Extensions;

public static class OrderMapper
{
    public static Address ToAddress(AddressDto addressDto) => Address.Of(
       firstName: addressDto.FirstName,
       lastName: addressDto.LastName,
       emailAddress: addressDto.EmailAddress,
       addressLine: addressDto.AddressLine,
       country: addressDto.Country,
       state: addressDto.State,
       zipCode: addressDto.ZipCode
   );

    public static Payment ToPayment(PaymentDto paymentDto) => Payment.Of(
        cardName: paymentDto.CardName,
        cardNumber: paymentDto.CardNumber,
        expiration: paymentDto.Expiration,
        cvv: paymentDto.Cvv,
        paymentMethod: paymentDto.PaymentMethod
    );
}
