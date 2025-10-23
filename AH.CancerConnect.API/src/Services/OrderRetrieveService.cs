namespace AH.CancerConnect.API.Services;

using AH.CancerConnect.API.Features.AuthToken;
using AH.CancerConnect.API.Features.OrderRetrieve;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

/// <inheritdoc />
public class OrderRetrieveService : IOrderRetrieveService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;
    private readonly XealthAppSettings _xealthAppSettings;

    public OrderRetrieveService(HttpClient httpClient, ITokenService tokenService, IOptions<XealthAppSettings> xealthAppSettings)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
        _xealthAppSettings = xealthAppSettings.Value;
    }

    /// <inheritdoc />
    public async Task<OrderResponse> GetOrderAsync(string deployment, string orderId)
    {
        var response = await _tokenService.GetBearerTokenAsync();
        if (response.access_token == null)
        {
            return new OrderResponse
            {
                Error = response,
                StatusCode = 400,
            };
        }

        var bearerToken = response.access_token;
        string url = $"{_xealthAppSettings.ApiBaseUrl}/read/order/{deployment}/{orderId}";
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

        var responseOrder = await _httpClient.GetAsync(url);
        var content = await responseOrder.Content.ReadAsStringAsync();

        if (responseOrder.IsSuccessStatusCode)
        {
            return new OrderResponse
            {
                Data = JsonSerializer.Deserialize<object>(content),
            };
        }

        return new OrderResponse
        {
            Error = JsonSerializer.Deserialize<object>(content),
            StatusCode = (int)responseOrder.StatusCode,
        };
    }
}