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
    /// Create drainage entries for multiple drains at once
    /// Example: POST /api/v1/drainage-entry
    /// Body: { 
    ///   "patientId": 123, 
    ///   "emptyDate": "2025-11-03T11:22:00", 
    ///   "drainEntries": [
    ///     { "drainId": 1, "amount": 45.5 },
    ///     { "drainId": 2, "amount": 38.2 }
    ///   ],
    ///   "note": "Patient feeling better" 
    /// }.
    /// Note: Amount must be between 0 and 100 mL for each drain.
    /// </summary>
    /// <param name="request">Drainage entry request with multiple drains.</param>
    /// <returns>Success response with created drainage entry IDs.</returns>
    [HttpPost]
    [ProducesResponseType<DrainageEntryResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostDrainageEntry([FromBody] DrainageEntryRequest request)
    {
        var entryIds = await _drainageEntryDataService.CreateDrainageEntryAsync(request);
        var response = new DrainageEntryResponse
        {
            EntryIds = entryIds,
            Message = $"{entryIds.Count} drainage entry(ies) created successfully",
        };

        _logger.LogDebug("Created {Count} drainage entries for patient {PatientId}", entryIds.Count, request.PatientId);
        return Ok(response);
    }

    /// <summary>
    /// Update multiple drainage entries in a session
    /// Example: PUT /api/v1/drainage-entry
    /// Body: { "emptyDate": "2025-11-05T10:30:00", "drainEntries": [{"drainId": 1, "amount": 30.5}, {"drainId": 2, "amount": 25.0}], "note": "Updated measurement" }.
    /// Note: Amount must be between 0 and 100 mL. Updates entries matching the drainId, emptyDate, and note.
    /// </summary>
    /// <param name="request">Updated drainage entry data with multiple drain entries.</param>
    /// <returns>Success response.</returns>
    [HttpPut]
    [ProducesResponseType<DrainageEntryResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateDrainageEntry([FromBody] DrainageEntryUpdateRequest request)
    {
        await _drainageEntryDataService.UpdateDrainageEntryAsync(request);
        var response = new DrainageEntryResponse
        {
            EntryIds = request.DrainEntries.Select(e => e.DrainId).ToList(),
            Message = $"{request.DrainEntries.Count} drainage entry/entries updated successfully",
        };

        _logger.LogDebug("{Count} drainage entries updated successfully", request.DrainEntries.Count);
        return Ok(response);
    }

    /// <summary>
    /// Get all drainage sessions for a patient (grouped by empty date and note)
    /// Example: GET /api/v1/drainage-entry/patient/123.
    /// Returns drainage entries grouped by session. Entries with the same EmptyDate and Note are grouped together,
    /// regardless of when they were created. This allows multiple drains emptied at the same time to appear as one session.
    /// Example Response:
    /// [
    ///   {
    ///     "DrainageEntryId": 5,
    ///     "PatientId": 1,
    ///     "EmptyDate": "2025-11-13T13:58:33.711181",
    ///     "DrainEntries": [
    ///       {
    ///         "DrainId": 2,
    ///         "Amount": 20,
    ///         "DrainName": "Left",
    ///         "IsArchived": false
    ///       },
    ///       {
    ///         "DrainId": 3,
    ///         "Amount": 10,
    ///         "DrainName": "Right",
    ///         "IsArchived": false
    ///       }
    ///     ],
    ///     "Note": "Morning drainage session"
    ///   }
    /// ].
    /// </summary>
    /// <param name="patientId">ID of the patient.</param>
    /// <returns>List of grouped drainage sessions for the patient.</returns>
    [HttpGet("patient/{patientId}")]
    [ProducesResponseType<IEnumerable<DrainageSessionResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDrainageEntry(int patientId)
    {
        var sessions = await _drainageEntryDataService.GetDrainageSessionsByPatientAsync(patientId);
        _logger.LogDebug("Retrieved {Count} drainage sessions for patient {PatientId}", sessions.Count(), patientId);
        return Ok(sessions);
    }

    /// <summary>
    /// Delete (archive) a drainage entry by ID
    /// Example: DELETE /api/v1/drainage-entry/123.
    /// </summary>
    /// <param name="entryId">ID of the drainage entry to delete.</param>
    /// <returns>Success response.</returns>
    [HttpDelete("{entryId}")]
    [ProducesResponseType<DrainageEntrySingleResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteDrainageEntry(int entryId)
    {
        await _drainageEntryDataService.DeleteDrainageEntryAsync(entryId);
        var response = new DrainageEntrySingleResponse
        {
            Id = entryId,
            Message = "Drainage entry deleted successfully",
        };

        _logger.LogDebug("Drainage entry {EntryId} deleted successfully", entryId);
        return Ok(response);
    }
    /// <summary>
    /// Get drainage graph data for a patient
    /// Example: GET /api/v1/drainage-entry/graph?patientId=123.
    /// Returns all drainage data for the patient. Graph data and alerts exclude today's entries.
    /// Returns:
    /// - TotalEntries: Total number of drainage entries (excluding today)
    /// - Alert: Alert enum (NONE=0, TWO_CONSECUTIVE_DAYS_INCREASED=1, LARGE_INCREASE=2, GOAL_REACHED=3)
    ///   * TWO_CONSECUTIVE_DAYS_INCREASED: Drainage increased for two consecutive days
    ///   * LARGE_INCREASE: Drainage increased more than 50 mL in a single day
    ///   * GOAL_REACHED: Patient reached their drainage goal for 2 consecutive days
    /// - DrainagesData: Array of daily drainage totals (sum of all entries per day) for chart display (excluding today)
    /// - TodayDrainageEntries: Today's drainage sessions (for list display, not included in graph or alerts).
    /// </summary>
    /// <param name="patientId">ID of the patient.</param>
    /// <returns>Drainage graph data with today's entries excluded from graph and alerts.</returns>
    [HttpGet("graph")]
    [ProducesResponseType<DrainageGraphResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDrainageGraph([FromQuery] int patientId)
    {
        var request = new DrainageGraphRequest
        {
            PatientId = patientId,
        };

        var graphData = await _drainageEntryDataService.GetDrainageGraphAsync(request);
        _logger.LogDebug(
            "Retrieved drainage graph for patient {PatientId} with {TotalEntries} entries",
            patientId,
            graphData.TotalEntries);
        return Ok(graphData);
    }
}