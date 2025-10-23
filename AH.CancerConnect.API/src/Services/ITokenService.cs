namespace AH.CancerConnect.API.Services;

using AH.CancerConnect.API.Features.AuthToken;

/// <summary>
/// Provides functionality to retrieve bearer tokens for authentication.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Asynchronously retrieves a bearer token from the authentication server.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a TokenResponse object with token details.
    /// </returns>
    Task<TokenResponse> GetBearerTokenAsync();
}