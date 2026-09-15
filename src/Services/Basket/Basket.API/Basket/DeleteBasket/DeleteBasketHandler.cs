using Basket.Core.Repositories;
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

public class DeleteBasketCommandHandler(IBasketRepository repository, ILogger<DeleteBasketCommandHandler> logger)
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken ct)
    {
        logger.LogInformation("Executing delete basket command. {@Command}", command);

        var isDeleted = await repository.DeleteBasket(command.UserName, ct);

        logger.LogInformation(
            "Basket deleted successfully. UserName: {@UserName}, IsDeleted: {@IsDeleted}",
            command.UserName,
            isDeleted);

        logger.LogInformation("Delete basket command executed successfully.");
        return new DeleteBasketResult(IsSuccess: true);
    }
}
