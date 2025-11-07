namespace AH.CancerConnect.API.Features.Drainage.DrainageSetup;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Drainage Setup Related Endpoints.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/drainage-setup")]
public class DrainageSetupController : ControllerBase
{
    private readonly IDrainageSetupDataService _drainageSetupDataService;
    private readonly ILogger<DrainageSetupController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DrainageSetupController"/> class.
    /// </summary>
    /// <param name="drainageSetupDataService">Drainage setup data service.</param>
    /// <param name="logger">Logger instance.</param>
    public DrainageSetupController(IDrainageSetupDataService drainageSetupDataService, ILogger<DrainageSetupController> logger)
    {
        _drainageSetupDataService = drainageSetupDataService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new drainage setup for a patient
    /// Example: POST /api/v1/drainage-setup
    /// Body: { "patientId": 123, "hasProviderGoalAmount": true, "goalDrainageAmount": 28, "providerInstructions": "Test", "drains": [{ "name": "Drain 1" }, { "name": "Drain 2" }] }.
    /// </summary>
    /// <param name="request">Drainage setup request.</param>
    /// <returns>Success response with drainage setup ID.</returns>
    [HttpPost]
    [ProducesResponseType<DrainageSetupResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostDrainageSetup([FromBody] DrainageSetupRequest request)
    {
        var setupId = await _drainageSetupDataService.CreateDrainageSetupAsync(request);
        var response = new DrainageSetupResponse
        {
            Id = setupId,
            Message = "Drainage setup created successfully",
        };

        _logger.LogDebug("Drainage setup created successfully with ID {SetupId}", setupId);
        return Ok(response);
    }

    /// <summary>
    /// Get drainage setup for a patient
    /// Example:
    /// - GET /api/v1/drainage-setup/patient/123 (returns drainage setup details for patient 123).
    /// </summary>
    /// <param name="patientId">Patient ID.</param>
    /// <returns>Drainage setup details.</returns>
    [HttpGet("patient/{patientId}")]
    [ProducesResponseType<DrainageSetupDetailResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDrainageSetupByPatient([Range(1, int.MaxValue)] int patientId)
    {
        _logger.LogDebug("GetDrainageSetupByPatient called for patient {PatientId}", patientId);

        var drainageSetup = await _drainageSetupDataService.GetDrainageSetupByPatientAsync(patientId);

        if (drainageSetup == null)
        {
            _logger.LogDebug("No drainage setup found for patient {PatientId}", patientId);
            throw new KeyNotFoundException($"No drainage setup found for patient {patientId}");
        }

        return Ok(drainageSetup);
    }

    /// <summary>
    /// Update an existing drainage setup for a patient
    /// Example: PUT /api/v1/drainage-setup
    /// Body: { "id": 1, "patientId": 123, "hasProviderGoalAmount": false, "providerInstructions": "Updated instructions", "drains": [{ "id": 1, "name": "Updated Drain 1" }, { "name": "New Drain" }] }.
    /// </summary>
    /// <param name="request">Drainage setup update request.</param>
    /// <returns>Success response with drainage setup ID.</returns>
    [HttpPut]
    [ProducesResponseType<DrainageSetupResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutDrainageSetup([FromBody] DrainageSetupUpdateRequest request)
    {
        var setupId = await _drainageSetupDataService.UpdateDrainageSetupAsync(request);
        var response = new DrainageSetupResponse
        {
            Id = setupId,
            Message = "Drainage setup updated successfully",
        };

        _logger.LogDebug("Drainage setup updated successfully with ID {SetupId}", setupId);
        return Ok(response);
    }

    /// <summary>
    /// Archive a drain and all its related entries
    /// Example: PATCH /api/v1/drainage-setup/archive-drain
    /// Body: { "drainId": 1, "patientId": 123 }.
    /// </summary>
    /// <param name="request">Archive drain request.</param>
    /// <returns>Success response.</returns>
    [HttpPatch("archive-drain")]
    [ProducesResponseType<ArchiveDrainResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ArchiveDrain([FromBody] ArchiveDrainRequest request)
    {
        var success = await _drainageSetupDataService.ArchiveDrainAsync(request);

        if (!success)
        {
            throw new InvalidOperationException("Failed to archive drain");
        }

        var response = new ArchiveDrainResponse
        {
            DrainId = request.DrainId,
            Message = "Drain archived successfully",
        };

        _logger.LogDebug("Drain {DrainId} archived successfully", request.DrainId);
        return Ok(response);
    }
}