namespace AH.CancerConnect.API.Features.WeatherForecast.v2;

using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Test weather endpoint, version 2.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "v2", "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
    ];

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get weather forecast.
    /// </summary>
    /// <returns>WeatherForecast</returns>
    [HttpGet]
    [ProducesResponseType<WeatherForecast>(StatusCodes.Status200OK)]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogInformation("Getting weather ");

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)],
        })
        .ToArray();
    }
}
