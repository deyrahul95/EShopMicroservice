using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlock.Exceptions.Handler;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception ex, CancellationToken ct)
    {
        logger.LogError(
            "Error Message: {@Message}, Time of occurrence {@Time}",
            ex.Message,
            DateTime.UtcNow);

        (string Detail, string Title, int StatusCode) details = ex switch
        {
            InternalServerException =>
            (
                ex.Message,
                ex.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status500InternalServerError
            ),
            ValidationException =>
            (
                ex.Message,
                ex.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            BadRequestException =>
            (
                ex.Message,
                ex.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            NotFoundException =>
            (
                ex.Message,
                ex.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status404NotFound
            ),
            _ =>
            (
                ex.Message,
                ex.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status500InternalServerError
            )
        };

        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Detail = details.Detail,
            Status = details.StatusCode,
            Instance = context.Request.Path
        };

        problemDetails.Extensions.Add("TraceId", context.TraceIdentifier);

        if (ex is ValidationException validationException)
        {
            problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
        }

        await context.Response.WriteAsJsonAsync(problemDetails, ct);
        return true;
    }
}
