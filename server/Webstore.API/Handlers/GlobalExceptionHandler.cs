using Microsoft.AspNetCore.Diagnostics;
using Webstore.API.Application.Common.DTOs;
using Webstore.API.Domain.Exceptions;

namespace Webstore.API.Handlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        var (statusCode, message) = exception switch
        {
            EntityNotFoundException notFound => (StatusCodes.Status404NotFound, notFound.Message),
            DomainException domain => (StatusCodes.Status400BadRequest, domain.Message),
            _ => (StatusCodes.Status500InternalServerError, "An error occurred")
        };

        var response = ApiErrorDetailsDto.Create(message);
        response.TraceId = httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}
