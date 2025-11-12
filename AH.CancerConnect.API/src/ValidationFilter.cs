using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AH.CancerConnect.API;

/// <summary>
/// Custom validation filter to return consistent ProblemDetails for model validation errors.
/// </summary>
public class ValidationFilter : IActionFilter
{
    private readonly ILogger<ValidationFilter> _logger;

    /// <summary>
    /// Initializes a new instance of the ValidationFilter class.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    public ValidationFilter(ILogger<ValidationFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            _logger.LogWarning("Model validation failed for action {Action}", context.ActionDescriptor.DisplayName);

            var errorMessages = context.ModelState
                .SelectMany(x => x.Value?.Errors ?? new List<Microsoft.AspNetCore.Mvc.ModelBinding.ModelError>())
                .Select(x => x.ErrorMessage)
                .Where(x => !string.IsNullOrEmpty(x))
                .ToArray();

            var response = new
            {
                title = "One or more validation errors occurred.",
                status = StatusCodes.Status400BadRequest,
                errors = errorMessages
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}