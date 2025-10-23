namespace AH.CancerConnect.API.Services;

using AH.CancerConnect.API.Features.AuthToken;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

/// <inheritdoc />
public class XealthTokenService : ITokenService
{
    private readonly HttpClient _httpClient;
    private readonly XealthAppSettings _xealthAppSettings;

    public XealthTokenService(HttpClient httpClient, IOptions<XealthAppSettings> xealthAppSettings)
    {
        _httpClient = httpClient;
        _xealthAppSettings = xealthAppSettings.Value;
    }

    /// <inheritdoc />
    public async Task<TokenResponse> GetBearerTokenAsync()
    {
        var url = _xealthAppSettings.TokenUrl;
        var username = _xealthAppSettings.ClientId;
        var password = _xealthAppSettings.ClientSecret;

        var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        var formData = "grant_type=client_credentials";
        var content = new StringContent(formData, Encoding.UTF8, "application/x-www-form-urlencoded");

        var response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var tokenDto = JsonSerializer.Deserialize<TokenResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });

        return tokenDto;
    }
}