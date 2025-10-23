namespace AH.CancerConnect.API.Services;

using AH.CancerConnect.API.Features.OrderEventNotification;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class OrderEventNotificationService : IOrderEventNotificationService
{

    public OrderEventNotificationService()
    {
    }

    public async Task<OrderEventNotificationResponse> OrderEventNotification(OrderEventNotificationRequest orderEventNotificationRequest)
    {
        OrderEventNotificationResponse orderEventNotificationResponse = new OrderEventNotificationResponse
        {
            EventId = orderEventNotificationRequest.EventId,
        };

        return await Task.FromResult(orderEventNotificationResponse);
    }
}