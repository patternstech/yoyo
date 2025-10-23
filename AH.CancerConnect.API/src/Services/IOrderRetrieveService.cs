namespace AH.CancerConnect.API.Services;

using AH.CancerConnect.API.Features.OrderRetrieve;

/// <summary>
/// Provides functionality to retrieve Order details.
/// </summary>
public interface IOrderRetrieveService
{
    /// <summary>
    /// Asynchronously retrieves an order detail using deployment and orderId parameters.
    /// </summary>
    /// <param name="deployment">The deployment identifier.</param>
    /// <param name="orderId">The order ID.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an OrderResponse.
    /// </returns>
    Task<OrderResponse> GetOrderAsync(string deployment, string orderId);
}
