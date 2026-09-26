using Ordering.Domain.Abstractions;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObjects;

namespace Ordering.Domain.Models;

public class Order : Aggregate<Guid>
{
    private readonly List<OrderItem> _orderItems = [];

    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public Guid CustomerId { get; private set; }
    public string OrderName { get; private set; } = string.Empty;
    public Address ShippingAddress { get; private set; } = default!;
    public Address BillingAddress { get; set; } = default!;
    public Payment Payment { get; private set; } = default!;
    public string Status { get; set; } = OrderStatus.Pending;
    public decimal TotalPrice
    {
        get => OrderItems.Sum(x => x.Price * x.Quantity);
        private set { }
    }
}