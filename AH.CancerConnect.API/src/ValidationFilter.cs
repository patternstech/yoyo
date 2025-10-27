using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AH.CancerConnect.API;

/// <summary>
/// Custom validation filter to return HTTP 422 Unprocessable Entity for model validation errors
/// instead of the default HTTP 400 Bad Request.
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

    /// <summary>
    /// Called before the action method is executed.
    /// </summary>
    /// <param name="context">Action executing context.</param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            _logger.LogWarning("Model validation failed for action {Action}", context.ActionDescriptor.DisplayName);

            var validationErrors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            var response = new
            {
                Success = false,
                Error = "Validation Error",
                ValidationErrors = validationErrors
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity
            };
        }
    }

    /// <summary>
    /// Called after the action method is executed.
    /// </summary>
    /// <param name="context">Action executed context.</param>
    public void OnActionExecuted(ActionExecutedContext context)
    {
        // No implementation needed for this filter
    }
}