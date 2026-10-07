using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Events;

namespace Ordering.Application.Orders.EventHandlers;

public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent notification, CancellationToken ct = default)
    {
        logger.LogInformation(
            "Handling order created domain event: {@Event}",
            JsonSerializer.Serialize(notification));

        // Apply any business logic needed for this event handler

        logger.LogInformation(
            "Domain Event Handled: {@DomainEvent}",
            notification.GetType().Name);
        return Task.CompletedTask;
    }
}
