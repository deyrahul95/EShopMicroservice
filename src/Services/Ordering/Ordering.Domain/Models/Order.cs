using Ordering.Domain.Abstractions;
using Ordering.Domain.Enums;
using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;
using Ordering.Domain.ValueObjects;

namespace Ordering.Domain.Models;

public class Order : Aggregate<OrderId>
{
    private readonly List<OrderItem> _orderItems = [];

    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public CustomerId CustomerId { get; private set; } = default!;
    public OrderName OrderName { get; private set; } = default!;
    public Address ShippingAddress { get; private set; } = default!;
    public Address BillingAddress { get; set; } = default!;
    public Payment Payment { get; private set; } = default!;
    public string Status { get; set; } = OrderStatus.Pending;
    public decimal TotalPrice
    {
        get => OrderItems.Sum(x => x.Price * x.Quantity);
        private set { }
    }

    public static Order Create(
        OrderId id,
        CustomerId customerId,
        OrderName orderName,
        Address shippingAddress,
        Address billingAddress,
        Payment payment)
    {
        var order = new Order
        {
            Id = id,
            CustomerId = customerId,
            OrderName = orderName,
            ShippingAddress = shippingAddress,
            BillingAddress = billingAddress,
            Payment = payment,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };

        order.AddDomainEvents(new OrderCreatedEvent(order));

        return order;
    }

    public void Update(
        OrderName orderName,
        Address shippingAddress,
        Address billingAddress,
        Payment payment,
        string orderStatus)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(orderStatus);

        if (OrderStatus.IsValid(orderStatus) == false)
        {
            throw new DomainException($"Invalid order status.");
        }

        OrderName = orderName;
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
        Payment = payment;
        Status = orderStatus;
        LastModified = DateTime.UtcNow;

        AddDomainEvents(new OrderUpdatedEvent(this));
    }

    public void Add(ProductId productId, int quantity, decimal price)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var orderItem = new OrderItem(
            orderId: Id,
            productId: productId,
            quantity: quantity,
            price: price)
        {
            CreatedAt = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };

        _orderItems.Add(orderItem);
    }

    public void Remove(ProductId productId)
    {
        var orderItem = _orderItems.FirstOrDefault(item => item.ProductId == productId);

        if (orderItem is not null)
        {
            _orderItems.Remove(orderItem);
        }

        LastModified = DateTime.UtcNow;
    }
}