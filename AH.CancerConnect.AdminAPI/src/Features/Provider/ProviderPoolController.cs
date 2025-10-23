using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AH.CancerConnect.AdminAPI.Features.Provider;

/// <summary>
/// Provider Pool Related Endpoints.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/provider-pools")]
public class ProviderPoolController : ControllerBase
{
    private readonly IProviderDataService _providerDataService;
    private readonly ILogger<ProviderPoolController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProviderPoolController"/> class.
    /// </summary>
    /// <param name="providerDataService">Provider data service.</param>
    /// <param name="logger">Logger instance.</param>
    public ProviderPoolController(IProviderDataService providerDataService, ILogger<ProviderPoolController> logger)
    {
        _providerDataService = providerDataService;
        _logger = logger;
    }

    /// <summary>
    /// Get all provider pools (for dropdowns and lists)
    /// Example: 
    /// - GET /api/v1/provider-pools (active pools only)
    /// </summary>
    /// <returns>List of provider pools.</returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<ProviderPoolListResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProviderPools()
    {
        _logger.LogDebug("GetProviderPools called");

        var providerPools = await _providerDataService.GetProviderPoolsAsync();
        return Ok(providerPools);
    }
}