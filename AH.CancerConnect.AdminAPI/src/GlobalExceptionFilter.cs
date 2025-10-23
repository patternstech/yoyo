namespace AH.CancerConnect.AdminAPI;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Global exception filter to handle exceptions across all controllers
/// </summary>
public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    /// <summary>
    /// Initializes a new instance of the GlobalExceptionFilter class
    /// </summary>
    /// <param name="logger">Logger instance</param>
    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles exceptions thrown during action execution
    /// </summary>
    /// <param name="context">Exception context</param>
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        // Handle different exception types
        var result = exception switch
        {
            ArgumentException argEx => HandleArgumentException(argEx),
            DbUpdateException dbEx => HandleDbUpdateException(dbEx),
            KeyNotFoundException notFoundEx => HandleNotFoundException(notFoundEx),
            UnauthorizedAccessException unauthorizedEx => HandleUnauthorizedException(unauthorizedEx),
            _ => HandleGenericException(exception)
        };

        context.Result = result;
        context.ExceptionHandled = true;
    }

    private ObjectResult HandleArgumentException(ArgumentException exception)
    {
        _logger.LogWarning(exception, "Validation error: {Message}", exception.Message);

        return new BadRequestObjectResult(new
        {
            Success = false,
            Error = "Validation Error",
            ValidationErrors = new List<string> { exception.Message }
        });
    }

    private ObjectResult HandleDbUpdateException(DbUpdateException exception)
    {
        _logger.LogWarning(exception, "Database constraint violation: {Message}", exception.Message);

        return new BadRequestObjectResult(new
        {
            Success = false,
            Error = "Database Error",
            ValidationErrors = new List<string> { "Error due to invalid data or constraint violation." }
        });
    }

    private ObjectResult HandleNotFoundException(KeyNotFoundException exception)
    {
        _logger.LogWarning(exception, "Resource not found: {Message}", exception.Message);

        return new NotFoundObjectResult(new
        {
            Success = false,
            Error = "Not Found",
            Message = exception.Message
        });
    }

    private ObjectResult HandleUnauthorizedException(UnauthorizedAccessException exception)
    {
        _logger.LogWarning(exception, "Unauthorized access: {Message}", exception.Message);

        return new ObjectResult(new
        {
            Success = false,
            Error = "Unauthorized",
            Message = "You are not authorized to perform this action."
        })
        {
            StatusCode = StatusCodes.Status401Unauthorized
        };
    }

    private ObjectResult HandleGenericException(Exception exception)
    {
        _logger.LogError(exception, "An unexpected error occurred: {Message}", exception.Message);

        return new ObjectResult(new
        {
            Success = false,
            Error = "Internal Server Error",
            Message = "An unexpected error occurred. Please try again later.",
            Details = exception.Message // Remove in production
        })
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}
