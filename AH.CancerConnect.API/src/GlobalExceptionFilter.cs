namespace AH.CancerConnect.API;

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
            InvalidOperationException invalidOpEx => HandleInvalidOperationException(invalidOpEx),
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

        var response = new
        {
            title = "One or more validation errors occurred.",
            status = StatusCodes.Status400BadRequest,
            errors = new[] { exception.Message }
        };

        return new BadRequestObjectResult(response);
    }

    private ObjectResult HandleInvalidOperationException(InvalidOperationException exception)
    {
        _logger.LogWarning(exception, "Invalid operation: {Message}", exception.Message);

        var response = new
        {
            title = "Invalid operation.",
            status = StatusCodes.Status400BadRequest,
            errors = new[] { exception.Message }
        };

        return new BadRequestObjectResult(response);
    }

    private ObjectResult HandleDbUpdateException(DbUpdateException exception)
    {
        _logger.LogWarning(exception, "Database constraint violation: {Message}", exception.Message);

        var response = new
        {
            title = "Database constraint violation.",
            status = StatusCodes.Status400BadRequest,
            errors = new[] { "Error due to invalid data or constraint violation." }
        };

        return new BadRequestObjectResult(response);
    }

    private ObjectResult HandleNotFoundException(KeyNotFoundException exception)
    {
        _logger.LogWarning(exception, "Resource not found: {Message}", exception.Message);

        var response = new
        {
            title = "Resource not found.",
            status = StatusCodes.Status404NotFound,
            errors = new[] { exception.Message }
        };

        return new NotFoundObjectResult(response);
    }

    private ObjectResult HandleUnauthorizedException(UnauthorizedAccessException exception)
    {
        _logger.LogWarning(exception, "Unauthorized access: {Message}", exception.Message);

        var response = new
        {
            title = "Unauthorized.",
            status = StatusCodes.Status401Unauthorized,
            errors = new[] { "You are not authorized to perform this action." }
        };

        return new ObjectResult(response)
        {
            StatusCode = StatusCodes.Status401Unauthorized
        };
    }

    private ObjectResult HandleGenericException(Exception exception)
    {
        _logger.LogError(exception, "An unexpected error occurred: {Message}", exception.Message);

        var response = new
        {
            title = "An error occurred while processing your request.",
            status = StatusCodes.Status500InternalServerError,
            errors = new[] { "An unexpected error occurred. Please try again later." }
        };

        return new ObjectResult(response)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}
