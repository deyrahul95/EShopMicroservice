using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlock.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct = default)
    {
        string requestTypeName = typeof(TRequest).Name;
        string responseTypeName = typeof(TResponse).Name;

        logger.LogInformation(
            "[START] Handle Request: {Request} - Response: {Response} - Request Data: {@RequestData}",
            requestTypeName,
            responseTypeName,
            request);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next(ct);

        timer.Stop();
        var timeTaken = timer.Elapsed;

        // If the request is greater than 3 seconds, then log this as performance warning
        if (timeTaken.Seconds > 3)
        {
            logger.LogWarning(
                "[PERFORMANCE] The request {Request} took {TimeTaken} seconds.",
                requestTypeName,
                timeTaken.Seconds);
        }

        logger.LogInformation(
            "[END] Handled {Request} with {Response} took {TimeTaken} milliseconds",
            requestTypeName,
            responseTypeName,
            timeTaken.Milliseconds);
        return response;
    }
}
