namespace AH.CancerConnect.API.Features.HealthCheck;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Health check endpoints
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class HealthCheckController : ControllerBase
{
    /// <summary>
    /// Get health check of application
    /// </summary>
    /// <returns>OK</returns>
    [HttpGet]
    public IActionResult GetHealth()
    {
        // TODO: check database

        // Add logic to check the health of your application
        return Ok(new { Status = "Healthy" });
    }
}
