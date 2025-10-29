namespace AH.CancerConnect.API.Features.SymptomsTracking;

using System.ComponentModel.DataAnnotations;
using AH.CancerConnect.API.Features.SymptomsTracking.Models;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Symptoms Related Endpoints.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class SymptomsController : ControllerBase
{
    private readonly ISymptomDataService _symptomDataService;
    private readonly ILogger<SymptomsController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SymptomsController"/> class.
    /// </summary>
    /// <param name="symptomDataService">Symptom data service.</param>
    /// <param name="logger">Logger instance.</param>
    public SymptomsController(ISymptomDataService symptomDataService, ILogger<SymptomsController> logger)
    {
        _symptomDataService = symptomDataService;
        _logger = logger;
    }

    /// <summary>
    /// Get symptoms with severity options
    /// Example:
    /// - GET /api/v1/symptoms (returns symptoms with severity options).
    /// </summary>
    /// <returns>List of symptoms with severity options.</returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<SymptomResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSymptoms()
    {
        _logger.LogDebug("GetSymptoms called");

        var symptoms = await _symptomDataService.GetSymptomsAsync();
        return Ok(symptoms);
    }

    /// <summary>
    /// Create symptom entry for a patient
    /// Example:
    /// POST /api/v1/symptoms
    /// Body: { "patientId": 123, "entryDate": "2025-09-17", "note": "Feeling better today", "symptomDetails": [{ "symptomId": 1, "categoryId": 2, "symptomValue": "Mild" }] }.
    /// </summary>
    /// <param name="request">Entry request containing patient symptom details and notes.</param>
    /// <returns>Success response with entry ID.</returns>
    [HttpPost]
    [ProducesResponseType<SymptomEntryResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostSymptoms([FromBody] SymptomEntryRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogDebug("PostSymptomEntry called for patient {PatientId} with {Count} details", request.PatientId, request.SymptomDetails.Count);

        var entryId = await _symptomDataService.CreateSymptomEntryAsync(request);

        return Ok(new SymptomEntryResponse
        {
            Success = true,
            EntryId = entryId,
            Count = request.SymptomDetails.Count,
        });
    }

    /// <summary>
    /// Update symptom entry for a patient
    /// Example:
    /// PUT /api/v1/symptoms
    /// Body: { "id": 1, "patientId": 123, "entryDate": "2025-09-17", "note": "Updated note", "symptomDetails": [{ "id": 1, "symptomId": 1, "categoryId": 2, "symptomValue": "Severe" }] }.
    /// </summary>
    /// <param name="request">Update request containing patient symptom details and notes.</param>
    /// <returns>Success response with entry ID.</returns>
    [HttpPut]
    [ProducesResponseType<SymptomEntryResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutSymptoms([FromBody] SymptomEntryUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogDebug("PutSymptomEntry called for entry {EntryId} with {Count} details", request.Id, request.SymptomDetails.Count);

        var entryId = await _symptomDataService.UpdateSymptomEntryAsync(request);

        return Ok(new SymptomEntryResponse
        {
            Success = true,
            EntryId = entryId,
            Count = request.SymptomDetails.Count,
        });
    }

    /// <summary>
    /// Delete symptom entry for a patient
    /// Example:
    /// DELETE /api/v1/symptoms/4?patientId=123.
    /// </summary>
    /// <param name="id">Entry ID to delete.</param>
    /// <param name="patientId">Patient ID for validation.</param>
    /// <returns>Success response.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType<SymptomEntryResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteSymptoms([Range(1, int.MaxValue)] int id, [FromQuery][Range(1, int.MaxValue)] int patientId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("DeleteSymptomEntry called for entry {EntryId} and patient {PatientId}", id, patientId);

        await _symptomDataService.DeleteSymptomEntryAsync(id, patientId);

        return Ok(new SymptomEntryResponse
        {
            Success = true,
            EntryId = id,
            Count = 0,
        });
    }

    /// <summary>
    /// Get symptom summary for a patient containing all symptom entries and their details
    /// Example:
    /// GET /api/v1/symptoms/summary?patientId=123.
    /// </summary>
    /// <param name="request">Request containing patient ID to retrieve symptom summary for.</param>
    /// <returns>List of all symptom entries with details for the patient.</returns>
    [HttpGet("summary")]
    [ProducesResponseType<IEnumerable<SymptomEntryDetailResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSymptomSummary([FromQuery] SymptomSummaryRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogDebug("GetSymptomSummary called for patient {PatientId}", request.PatientId);

        var summary = await _symptomDataService.GetSymptomSummaryAsync(request.PatientId);

        return Ok(summary);
    }

    /// <summary>
    /// Get symptom graph data for a patient within a date range
    /// Example:
    /// GET /api/v1/symptoms/graph?patientId=123&amp;days=7
    /// GET /api/v1/symptoms/graph?patientId=123&amp;days=30
    /// </summary>
    /// <param name="patientId">Patient ID to retrieve graph data for.</param>
    /// <param name="days">Number of days to look back from today.</param>
    /// <returns>Graph data for symptoms within the specified date range.</returns>
    [HttpGet("graph")]
    [ProducesResponseType<SymptomGraphResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSymptomGraphData(
        [FromQuery][Required][Range(1, int.MaxValue)] int patientId,
        [FromQuery][Required] int days)
    {
        _logger.LogDebug(
            "GetSymptomGraphData called for patient {PatientId} with {Days} days",
            patientId, days);

        var graphData = await _symptomDataService.GetSymptomGraphDataAsync(patientId, days);

        return Ok(graphData);
    }
}
