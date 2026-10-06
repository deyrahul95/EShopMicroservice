using Ordering.Application.Dtos;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Extensions;

public static class OrderExtensions
{
    public static Address ToAddress(this AddressDto addressDto) => Address.Of(
       firstName: addressDto.FirstName,
       lastName: addressDto.LastName,
       emailAddress: addressDto.EmailAddress,
       addressLine: addressDto.AddressLine,
       country: addressDto.Country,
       state: addressDto.State,
       zipCode: addressDto.ZipCode);

    public static AddressDto ToDto(this Address address) => new(
        FirstName: address.FirstName,
        LastName: address.LastName,
        EmailAddress: address.EmailAddress,
        AddressLine: address.AddressLine,
        Country: address.Country,
        State: address.State,
        ZipCode: address.ZipCode);

    public static Payment ToPayment(this PaymentDto paymentDto) => Payment.Of(
        cardName: paymentDto.CardName,
        cardNumber: paymentDto.CardNumber,
        expiration: paymentDto.Expiration,
        cvv: paymentDto.Cvv,
        paymentMethod: paymentDto.PaymentMethod);

    public static PaymentDto ToDto(this Payment payment) => new(
        CardName: payment.CardName,
        CardNumber: payment.CardNumber,
        Expiration: payment.Expiration,
        Cvv: payment.CVV,
        PaymentMethod: payment.PaymentMethod);

    public static OrderItemDto ToDto(this OrderItem orderItem) => new(
        OrderId: orderItem.OrderId.Value,
        ProductId: orderItem.ProductId.Value,
        Quantity: orderItem.Quantity,
        Price: orderItem.Price);

    public static OrderDto ToDto(this Order order) => new(
        Id: order.Id.Value,
        CustomerId: order.CustomerId.Value,
        OrderName: order.OrderName.Value,
        ShippingAddress: order.ShippingAddress.ToDto(),
        BillingAddress: order.BillingAddress.ToDto(),
        Payment: order.Payment.ToDto(),
        OrderStatus: order.Status,
        OrderItems: [.. order.OrderItems.Select(ToDto)]);

    public static IEnumerable<OrderDto> ToDtoList(this IEnumerable<Order> orders) => orders.Select(ToDto);
}
