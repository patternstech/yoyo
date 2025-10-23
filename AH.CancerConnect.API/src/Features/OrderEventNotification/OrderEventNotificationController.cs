namespace AH.CancerConnect.API.Features.OrderEventNotification;

using AH.CancerConnect.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

/// <summary>
/// Order event notification endpoints.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class OrderEventNotificationController : ControllerBase
{
    private readonly ILogger<OrderEventNotificationController> _logger;

    private readonly IOrderEventNotificationService _orderEventNotificationService;

    public OrderEventNotificationController(ILogger<OrderEventNotificationController> logger, IOrderEventNotificationService orderEventNotificationService)
    {
        _logger = logger;
        _orderEventNotificationService = orderEventNotificationService;
    }

    /// <summary>
    /// Receives order event notification from Xealth.
    /// </summary>
    /// <param name="orderEventNotificationRequest">The incoming order event notification payload.</param>
    /// <returns>HTTP 200 OK if successful, or 400 Bad Request if the payload is invalid., 500 StatusCode if error</returns>
    [HttpPost]
    [Route("OrderEventNotify")]
    public async Task<IActionResult> OrderEventNotification([FromBody] OrderEventNotificationRequest orderEventNotificationRequest)
    {
        try
        {
            // Validate the incoming request
            if (orderEventNotificationRequest == null)
            {
                return BadRequest(new OrderEventNotificationResponse
                {
                    Status = "400",
                    Message = "Invalid data.",
                });
            }

            // Simulate async processing (e.g., saving to database, calling another service)
            var orderEventNotificationResponse = await _orderEventNotificationService.OrderEventNotification(orderEventNotificationRequest);

            // Return a success response
            return Ok(new OrderEventNotificationResponse
            {
                Status = "200",
                Message = "Order event received successfully.",
                EventId = orderEventNotificationResponse.EventId,
            });
        }
        catch (Exception ex)
        {
            // Log the exception (e.g., using ILogger)
            _logger.LogError(string.Format("{0}, Message:{1}", "api:OrderEventNotify", ex.Message));
            return StatusCode(new OrderEventNotificationResponse
            {
                Status = "500",
                Message = "An error occurred while processing the order.",
            });
        }
    }

    private IActionResult StatusCode(OrderEventNotificationResponse orderEventNotificationResponse)
    {
        throw new NotImplementedException();
    }
}
