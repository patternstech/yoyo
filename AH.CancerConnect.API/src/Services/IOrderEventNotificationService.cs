namespace AH.CancerConnect.API.Services;

using AH.CancerConnect.API.Features.OrderEventNotification;

/// <summary>
/// Provides functionality for Order event notification from Xealth.
/// </summary>
public interface IOrderEventNotificationService
{
    /// <summary>
    /// Asynchronously Order event notification from Xealth.
    /// </summary>
    /// <param name="orderEventNotificationRequest">The task request contains a OrderEventNotificationRequest object with order details.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. Task result contains a OrderEventNotificationResponse object about order notification received successfully or not.
    /// </returns>
    Task<OrderEventNotificationResponse> OrderEventNotification(OrderEventNotificationRequest orderEventNotificationRequest);
}