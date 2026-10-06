using BuildingBlock.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;

namespace Ordering.Application.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderResult>;
public record UpdateOrderResult(bool IsSuccess);

public class UpdateOrderCommandValidation : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidation()
    {
        RuleFor(x => x.Order.Id)
            .NotEmpty()
            .WithMessage("Id is required");
        RuleFor(x => x.Order.OrderName)
            .NotEmpty()
            .WithMessage("Name is required");
        RuleFor(x => x.Order.CustomerId)
            .NotNull()
            .WithMessage("CustomerId is required");
    }
}
