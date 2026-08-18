using System.Text.Json;
using BuildingBlock.CQRS;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlock.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        logger.LogInformation(
            "Executing request validation for {RequestType}. {@Request}",
            typeof(TRequest).Name,
            request);

        if (!validators.Any())
        {
            logger.LogDebug("No validators found for request type {RequestType}",
                typeof(TRequest).Name);
            return await next(ct);
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            validators.Select(
                v => v.ValidateAsync(
                    context: context,
                    cancellation: ct)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
        {
            logger.LogWarning(
                "Request validation errors found for {RequestType}. {@Errors}",
                typeof(TRequest).Name,
                JsonSerializer.Serialize(failures));
            throw new ValidationException(failures);
        }

        logger.LogInformation(
            "Request validation completed for {RequestType}. Executing next middleware...",
            typeof(TRequest).Name);
        return await next(ct);
    }
}
