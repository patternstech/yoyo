using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AH.CancerConnect.AdminAPI.Features.Provider;

/// <summary>
/// Provider Related Endpoints.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/providers")]
public class ProviderController : ControllerBase
{
    private readonly IProviderDataService _providerDataService;
    private readonly ILogger<ProviderController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProviderController"/> class.
    /// </summary>
    /// <param name="providerDataService">Provider data service.</param>
    /// <param name="logger">Logger instance.</param>
    public ProviderController(IProviderDataService providerDataService, ILogger<ProviderController> logger)
    {
        _providerDataService = providerDataService;
        _logger = logger;
    }

    /// <summary>
    /// Get all providers.
    /// Example: 
    /// - GET /api/v1/providers.
    /// </summary>
    /// <returns>List of providers.</returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<ProviderDetailResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProviders()
    {
        _logger.LogDebug("GetProviders called}");

        var result = await _providerDataService.GetProvidersAsync();
        return Ok(result);
    }

    /// <summary>
    /// Create a new provider
    /// Example: POST /api/v1/providers
    /// Body: { "firstName": "John", "lastName": "Doe", "providerPoolId": 1, "isActive": true }.
    /// </summary>
    /// <param name="request">Provider creation request.</param>
    /// <returns>Success response with provider ID.</returns>
    [HttpPost]
    [ProducesResponseType<ProviderResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostProvider([FromBody] ProviderRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for provider creation: {ModelState}", ModelState);
            return BadRequest(ModelState);
        }

        try
        {
            var providerId = await _providerDataService.CreateProviderAsync(request);
            var response = new ProviderResponse
            {
                Id = providerId,
                Message = "Provider created successfully",
            };

            _logger.LogDebug("Provider created successfully with ID {ProviderId}", providerId);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Invalid argument while creating provider: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete a provider (soft delete - marks as inactive)
    /// Example: DELETE /api/v1/providers/1
    /// </summary>
    /// <param name="id">Provider ID.</param>
    /// <returns>Success response.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType<DeleteProviderResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProvider([Range(1, int.MaxValue)] int id)
    {
        _logger.LogDebug("DeleteProvider called for provider {ProviderId}", id);

        try
        {
            var success = await _providerDataService.DeleteProviderAsync(id);

            if (!success)
            {
                return NotFound(new { message = $"Provider with ID {id} not found" });
            }

            var response = new DeleteProviderResponse
            {
                Id = id,
                Message = "Provider deleted successfully",
                Success = true,
            };

            _logger.LogDebug("Provider {ProviderId} deleted successfully", id);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Invalid argument while deleting provider: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing provider
    /// Example: PUT /api/v1/providers
    /// Body: { "id": 1, "firstName": "John", "lastName": "Smith", "providerId": "12345", "providerPoolId": 2, "isActive": true }.
    /// </summary>
    /// <param name="request">Provider update request.</param>
    /// <returns>Success response with provider ID.</returns>
    [HttpPut]
    [ProducesResponseType<ProviderResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutProvider([FromBody] ProviderUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for provider update: {ModelState}", ModelState);
            return BadRequest(ModelState);
        }

        try
        {
            var providerId = await _providerDataService.UpdateProviderAsync(request);
            var response = new ProviderResponse
            {
                Id = providerId,
                Message = "Provider updated successfully",
            };

            _logger.LogDebug("Provider updated successfully with ID {ProviderId}", providerId);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Invalid argument while updating provider: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }
}