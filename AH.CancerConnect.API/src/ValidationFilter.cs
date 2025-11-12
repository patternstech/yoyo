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

            var errorMessages = new List<string>();
            
            foreach (var keyValuePair in context.ModelState)
            {
                var messages = keyValuePair.Value?.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList() ?? new List<string>();
                
                errorMessages.AddRange(messages);
            }

            var response = new
            {
                title = "One or more validation errors occurred.",
                status = StatusCodes.Status400BadRequest,
                errors = errorMessages
            };

            context.Result = new BadRequestObjectResult(response);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}