using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AH.CancerConnect.AdminAPI.Features.ProviderPool;

/// <summary>
/// Provider Pool Related Endpoints.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/provider-pools")]
public class ProviderPoolController : ControllerBase
{
    private readonly IProviderPoolDataService _providerPoolDataService;
    private readonly ILogger<ProviderPoolController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProviderPoolController"/> class.
    /// </summary>
    /// <param name="providerPoolDataService">Provider pool data service.</param>
    /// <param name="logger">Logger instance.</param>
    public ProviderPoolController(IProviderPoolDataService providerPoolDataService, ILogger<ProviderPoolController> logger)
    {
        _providerPoolDataService = providerPoolDataService;
        _logger = logger;
    }

    /// <summary>
    /// Get all provider pools (for dropdowns and lists)
    /// Example: 
    /// - GET /api/v1/provider-pools
    /// </summary>
    /// <returns>List of provider pools.</returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<ProviderPoolListResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProviderPools()
    {
        _logger.LogDebug("GetProviderPools called");

        var providerPools = await _providerPoolDataService.GetProviderPoolsAsync();
        return Ok(providerPools);
    }

    /// <summary>
    /// Create a new provider pool
    /// Example:
    /// POST /api/v1/provider-pools
    /// Body: { "name": "Oncology Team A", "description": "Primary oncology care team" }
    /// </summary>
    /// <param name="request">The provider pool creation request.</param>
    /// <returns>Success response with provider pool ID.</returns>
    [HttpPost]
    [ProducesResponseType<ProviderPoolResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProviderPool([FromBody] ProviderPoolRequest request)
    {
        _logger.LogDebug("CreateProviderPool called for {Name}", request.Name);

        // Get the logged-in user's name (for now, using a placeholder)
        // In production, this would come from the authenticated user context
        var createdBy = User.Identity?.Name ?? "System";

        var providerPoolId = await _providerPoolDataService.CreateProviderPoolAsync(request, createdBy);

        return Ok(new ProviderPoolResponse
        {
            Success = true,
            Id = providerPoolId,
        });
    }

    /// <summary>
    /// Update an existing provider pool
    /// Example:
    /// PUT /api/v1/provider-pools
    /// Body: { "id": 1, "name": "Updated Oncology Team", "description": "Updated description" }
    /// </summary>
    /// <param name="request">The provider pool update request.</param>
    /// <returns>Success response with provider pool ID.</returns>
    [HttpPut]
    [ProducesResponseType<ProviderPoolResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProviderPool([FromBody] ProviderPoolUpdateRequest request)
    {
        _logger.LogDebug("UpdateProviderPool called for ID {Id}", request.Id);

        var providerPoolId = await _providerPoolDataService.UpdateProviderPoolAsync(request);

        return Ok(new ProviderPoolResponse
        {
            Success = true,
            Id = providerPoolId,
        });
    }

    /// <summary>
    /// Delete a provider pool
    /// Example:
    /// DELETE /api/v1/provider-pools/1
    /// </summary>
    /// <param name="id">The provider pool ID to delete.</param>
    /// <returns>Success response.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType<ProviderPoolResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProviderPool([Range(1, int.MaxValue)] int id)
    {
        _logger.LogDebug("DeleteProviderPool called for ID {Id}", id);

        var success = await _providerPoolDataService.DeleteProviderPoolAsync(id);

        if (!success)
        {
            return NotFound(new { message = $"Provider pool with ID {id} not found" });
        }

        return Ok(new ProviderPoolResponse
        {
            Success = true,
            Id = id,
        });
    }
}
