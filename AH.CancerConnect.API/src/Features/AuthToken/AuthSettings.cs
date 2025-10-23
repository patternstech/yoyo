namespace AH.CancerConnect.API.Features.AuthToken;

public class AuthSettings
{
    public string? ClientId { get; set; }

    public string? SecretKey { get; set; }

    public int ExpiryDurationMinutes { get; set; }

    public string? Issuer { get; set; }
}