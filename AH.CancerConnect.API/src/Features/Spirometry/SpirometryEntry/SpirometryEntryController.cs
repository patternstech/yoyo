namespace AH.CancerConnect.API.Features.Spirometry.SpirometryEntry;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Spirometry Entry Related Endpoints.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/spirometry-entry")]
public class SpirometryEntryController : ControllerBase
{
    private readonly ISpirometryEntryDataService _spirometryEntryDataService;
    private readonly ILogger<SpirometryEntryController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpirometryEntryController"/> class.
    /// </summary>
    /// <param name="spirometryEntryDataService">Spirometry entry data service.</param>
    /// <param name="logger">Logger instance.</param>
    public SpirometryEntryController(ISpirometryEntryDataService spirometryEntryDataService, ILogger<SpirometryEntryController> logger)
    {
        _spirometryEntryDataService = spirometryEntryDataService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new spirometry entry for a patient
    /// Example: POST /api/v1/spirometry-entry
    /// Body: { "patientId": 123, "testDate": "2025-11-10", "testTime": "08:00:00", "numberReached": 2800, "note": "Morning test" }.
    /// Note: Number reached must be between 0.01 and 10000 mL.
    /// </summary>
    /// <param name="request">Spirometry entry request.</param>
    /// <returns>Success response with spirometry entry ID.</returns>
    [HttpPost]
    [ProducesResponseType<SpirometryEntryResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostSpirometryEntry([FromBody] SpirometryEntryRequest request)
    {
        var entryId = await _spirometryEntryDataService.CreateSpirometryEntryAsync(request);
        var response = new SpirometryEntryResponse
        {
            Id = entryId,
            Message = "Spirometry entry created successfully",
        };

        _logger.LogDebug("Spirometry entry created successfully with ID {EntryId}", entryId);
        return Ok(response);
    }

    /// <summary>
    /// Update an existing spirometry entry
    /// Example: PUT /api/v1/spirometry-entry/123
    /// Body: { "testDate": "2025-11-10", "testTime": "08:30:00", "numberReached": 3000, "note": "Updated reading" }.
    /// Note: Number reached must be between 0.01 and 10000 mL.
    /// </summary>
    /// <param name="entryId">ID of the spirometry entry to update.</param>
    /// <param name="request">Updated spirometry entry data.</param>
    /// <returns>Success response.</returns>
    [HttpPut("{entryId}")]
    [ProducesResponseType<SpirometryEntryResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateSpirometryEntry(int entryId, [FromBody] SpirometryEntryUpdateRequest request)
    {
        await _spirometryEntryDataService.UpdateSpirometryEntryAsync(entryId, request);
        var response = new SpirometryEntryResponse
        {
            Id = entryId,
            Message = "Spirometry entry updated successfully",
        };

        _logger.LogDebug("Spirometry entry {EntryId} updated successfully", entryId);
        return Ok(response);
    }

    /// <summary>
    /// Get spirometry graph data for a patient with all screen information
    /// Example: GET /api/v1/spirometry-entry/patient/123?days=7
    /// Returns everything needed for the graph screen: capacity goal, provider instructions, days tracked count, and today's entries.
    /// </summary>
    /// <param name="patientId">ID of the patient.</param>
    /// <param name="days">Number of days to include in graph (default: 7, max: 365).</param>
    /// <returns>Complete spirometry graph data with capacity goal, instructions, days tracked, and today's entries.</returns>
    [HttpGet("patient/{patientId}")]
    [ProducesResponseType<SpirometryGraphResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSpirometryGraph(int patientId, [FromQuery] int days = 7)
    {
        var request = new SpirometryGraphRequest
        {
            PatientId = patientId,
            Days = days,
        };

        var graphData = await _spirometryEntryDataService.GetSpirometryGraphAsync(request);
        _logger.LogDebug("Retrieved spirometry graph data for patient {PatientId}", patientId);
        return Ok(graphData);
    }

    /// <summary>
    /// Delete a spirometry entry by ID
    /// Example: DELETE /api/v1/spirometry-entry/123.
    /// </summary>
    /// <param name="entryId">ID of the spirometry entry to delete.</param>
    /// <returns>Success response.</returns>
    [HttpDelete("{entryId}")]
    [ProducesResponseType<SpirometryEntryResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteSpirometryEntry(int entryId)
    {
        await _spirometryEntryDataService.DeleteSpirometryEntryAsync(entryId);
        var response = new SpirometryEntryResponse
        {
            Id = entryId,
            Message = "Spirometry entry deleted successfully",
        };

        _logger.LogDebug("Spirometry entry {EntryId} deleted successfully", entryId);
        return Ok(response);
    }
}
