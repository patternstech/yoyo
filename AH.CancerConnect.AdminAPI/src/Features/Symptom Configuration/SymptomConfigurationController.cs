using Microsoft.AspNetCore.Mvc;

namespace AH.CancerConnect.AdminAPI.Features.SymptomConfiguration;

/// <summary>
/// Symptom Configuration Related Endpoints.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/symptom-configurations")]
public class SymptomConfigurationController : ControllerBase
{
    private readonly ISymptomConfigurationDataService _symptomConfigurationDataService;
    private readonly ILogger<SymptomConfigurationController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SymptomConfigurationController"/> class.
    /// </summary>
    /// <param name="symptomConfigurationDataService">Symptom configuration data service.</param>
    /// <param name="logger">Logger instance.</param>
    public SymptomConfigurationController(
        ISymptomConfigurationDataService symptomConfigurationDataService,
        ILogger<SymptomConfigurationController> logger)
    {
        _symptomConfigurationDataService = symptomConfigurationDataService;
        _logger = logger;
    }

    /// <summary>
    /// Get all symptom configurations.
    /// Example: 
    /// - GET /api/v1/symptom-configurations.
    /// </summary>
    /// <returns>List of symptom configurations.</returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<SymptomConfigurationResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSymptomConfigurations()
    {
        _logger.LogDebug("GetSymptomConfigurations called");

        var result = await _symptomConfigurationDataService.GetSymptomConfigurationsAsync();
        return Ok(result);
    }
}
