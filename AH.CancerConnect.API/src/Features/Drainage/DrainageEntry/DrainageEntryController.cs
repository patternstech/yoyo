namespace AH.CancerConnect.API.Features.Drainage.DrainageEntry;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Drainage Entry Related Endpoints.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/drainage-entry")]
public class DrainageEntryController : ControllerBase
{
    private readonly IDrainageEntryDataService _drainageEntryDataService;
    private readonly ILogger<DrainageEntryController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DrainageEntryController"/> class.
    /// </summary>
    /// <param name="drainageEntryDataService">Drainage entry data service.</param>
    /// <param name="logger">Logger instance.</param>
    public DrainageEntryController(IDrainageEntryDataService drainageEntryDataService, ILogger<DrainageEntryController> logger)
    {
        _drainageEntryDataService = drainageEntryDataService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new drainage entry for all drains in a drainage setup
    /// Example: POST /api/v1/drainage-entry
    /// Body: { "drainageSetupId": 1, "emptyDate": "2025-11-03T11:22:00", "drain1Amount": 25.5, "drain2Amount": 30.0, "drain3Amount": null, "drain4Amount": null, "note": "Patient feeling better" }.
    /// </summary>
    /// <param name="request">Drainage entry request.</param>
    /// <returns>Success response with drainage entry ID.</returns>
    [HttpPost]
    [ProducesResponseType<DrainageEntryResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostDrainageEntry([FromBody] DrainageEntryRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for drainage entry creation: {ModelState}", ModelState);
            return BadRequest(ModelState);
        }

        try
        {
            var entryId = await _drainageEntryDataService.CreateDrainageEntryAsync(request);
            var response = new DrainageEntryResponse
            {
                Id = entryId,
                Message = "Drainage entry created successfully",
            };

            _logger.LogDebug("Drainage entry created successfully with ID {EntryId}", entryId);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Invalid operation while creating drainage entry: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Invalid argument while creating drainage entry: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing drainage entry
    /// Example: PUT /api/v1/drainage-entry/123
    /// Body: { "emptyDate": "2025-11-05T10:30:00", "amount": 30.5, "note": "Updated measurement" }.
    /// </summary>
    /// <param name="entryId">ID of the drainage entry to update.</param>
    /// <param name="request">Updated drainage entry data.</param>
    /// <returns>Success response.</returns>
    [HttpPut("{entryId}")]
    [ProducesResponseType<DrainageEntryResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateDrainageEntry(int entryId, [FromBody] DrainageEntryUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for drainage entry update: {ModelState}", ModelState);
            return BadRequest(ModelState);
        }

        try
        {
            await _drainageEntryDataService.UpdateDrainageEntryAsync(entryId, request);
            var response = new DrainageEntryResponse
            {
                Id = entryId,
                Message = "Drainage entry updated successfully",
            };

            _logger.LogDebug("Drainage entry {EntryId} updated successfully", entryId);
            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Drainage entry not found: {Message}", ex.Message);
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Invalid operation while updating drainage entry: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Invalid argument while updating drainage entry: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get a drainage entry by ID
    /// Example: GET /api/v1/drainage-entry/123.
    /// </summary>
    /// <param name="entryId">ID of the drainage entry.</param>
    /// <returns>Drainage entry details.</returns>
    [HttpGet("{entryId}")]
    [ProducesResponseType<DrainageEntryDetailResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDrainageEntry(int entryId)
    {
        try
        {
            var entry = await _drainageEntryDataService.GetDrainageEntryByIdAsync(entryId);
            _logger.LogDebug("Retrieved drainage entry {EntryId}", entryId);
            return Ok(entry);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Drainage entry not found: {Message}", ex.Message);
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete (archive) a drainage entry by ID
    /// Example: DELETE /api/v1/drainage-entry/123.
    /// </summary>
    /// <param name="entryId">ID of the drainage entry to delete.</param>
    /// <returns>Success response.</returns>
    [HttpDelete("{entryId}")]
    [ProducesResponseType<DrainageEntryResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteDrainageEntry(int entryId)
    {
        try
        {
            await _drainageEntryDataService.DeleteDrainageEntryAsync(entryId);
            var response = new DrainageEntryResponse
            {
                Id = entryId,
                Message = "Drainage entry deleted successfully",
            };

            _logger.LogDebug("Drainage entry {EntryId} deleted successfully", entryId);
            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Drainage entry not found: {Message}", ex.Message);
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Invalid operation while deleting drainage entry: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }
}