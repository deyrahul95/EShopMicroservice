using BuildingBlock.CQRS;
using FluentValidation;

namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;
public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .NotNull()
            .WithMessage("Username is required.");
    }
}

public class DeleteBasketCommandHandler(ILogger<DeleteBasketCommandHandler> logger)
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken ct)
    {
        logger.LogInformation("Executing delete basket command. @{Command}", command);
        await Task.Delay(20, ct);

        // TODO: Delete the basket from the database and cache
        logger.LogInformation("Basket delete successfully. UserName: {@UserName}", command.UserName);
        logger.LogInformation("Delete basket command executed successfully.");
        return new DeleteBasketResult(IsSuccess: true);
    }
}
