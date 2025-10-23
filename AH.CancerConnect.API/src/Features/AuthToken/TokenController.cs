namespace AH.CancerConnect.API.Features.AuthToken;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// [Authorize]
[ApiController]
[Route("api")]
public class TokenController : ControllerBase
{
    private readonly AuthSettings _authSettings;

    public TokenController(IOptions<AuthSettings> authSettings)
    {
        _authSettings = authSettings.Value;
    }

    [HttpPost]
    [Route("token")]
    public IActionResult GenerateToken(string clientid, string secretKey)
    {
        if (clientid != _authSettings.ClientId || secretKey != _authSettings.SecretKey)
        {
            return Unauthorized();
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[] { new Claim(ClaimTypes.Name, clientid) };

        var token = new JwtSecurityToken(
            issuer: _authSettings.Issuer,
            audience: _authSettings.Issuer,
            expires: DateTime.Now.AddMinutes(_authSettings.ExpiryDurationMinutes),
            signingCredentials: creds);

        string gentoken = new JwtSecurityTokenHandler().WriteToken(token);

        // Return in OK Result
        return Ok(new
        {
            token = gentoken,
            expires = token.ValidTo.ToLocalTime(),
            token_type = "Bearer",
        });
    }
}