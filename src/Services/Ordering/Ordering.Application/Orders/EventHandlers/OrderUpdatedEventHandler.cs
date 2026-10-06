using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Events;

namespace Ordering.Application.Orders.EventHandlers;

public class OrderUpdatedEventHandler(ILogger<OrderUpdatedEventHandler> logger)
    : INotificationHandler<OrderUpdatedEvent>
{
    public Task Handle(OrderUpdatedEvent notification, CancellationToken ct = default)
    {
        logger.LogInformation("Handling order update domain event: {@Event}", notification);

        // Apply any business logic needed for this event handler

        logger.LogInformation("Domain Event Handled: {@DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}