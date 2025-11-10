namespace AH.CancerConnect.API.Features.Spirometry.SpirometrySetup;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Spirometry Setup Related Endpoints.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/spirometry-setup")]
public class SpirometrySetupController : ControllerBase
{
    private readonly ISpirometrySetupDataService _spirometrySetupDataService;
    private readonly ILogger<SpirometrySetupController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpirometrySetupController"/> class.
    /// </summary>
    /// <param name="spirometrySetupDataService">Spirometry setup data service.</param>
    /// <param name="logger">Logger instance.</param>
    public SpirometrySetupController(ISpirometrySetupDataService spirometrySetupDataService, ILogger<SpirometrySetupController> logger)
    {
        _spirometrySetupDataService = spirometrySetupDataService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new spirometry setup for a patient
    /// Example: POST /api/v1/spirometry-setup
    /// Body: { "patientId": 123, "capacityGoal": 1500, "providerInstructions": "Track spirometry at least 3 times a day" }.
    /// Note: Capacity goal is required and must be between 0.01 and 5000 mL.
    /// </summary>
    /// <param name="request">Spirometry setup request.</param>
    /// <returns>Success response with spirometry setup ID.</returns>
    [HttpPost]
    [ProducesResponseType<SpirometrySetupResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostSpirometrySetup([FromBody] SpirometrySetupRequest request)
    {
        var setupId = await _spirometrySetupDataService.CreateSpirometrySetupAsync(request);
        var response = new SpirometrySetupResponse
        {
            Id = setupId,
            Message = "Spirometry setup created successfully",
        };

        _logger.LogDebug("Spirometry setup created successfully with ID {SetupId}", setupId);
        return Ok(response);
    }

    /// <summary>
    /// Get spirometry setup for a patient
    /// Example:
    /// - GET /api/v1/spirometry-setup/patient/123 (returns spirometry setup details for patient 123).
    /// </summary>
    /// <param name="patientId">Patient ID.</param>
    /// <returns>Spirometry setup details.</returns>
    [HttpGet("patient/{patientId}")]
    [ProducesResponseType<SpirometrySetupDetailResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSpirometrySetupByPatient([Range(1, int.MaxValue)] int patientId)
    {
        _logger.LogDebug("GetSpirometrySetupByPatient called for patient {PatientId}", patientId);

        var spirometrySetup = await _spirometrySetupDataService.GetSpirometrySetupByPatientAsync(patientId);

        if (spirometrySetup == null)
        {
            _logger.LogDebug("No spirometry setup found for patient {PatientId}", patientId);
            throw new KeyNotFoundException($"No spirometry setup found for patient {patientId}");
        }

        return Ok(spirometrySetup);
    }

    /// <summary>
    /// Update an existing spirometry setup for a patient
    /// Example: PUT /api/v1/spirometry-setup
    /// Body: { "id": 1, "patientId": 123, "capacityGoal": 2000, "providerInstructions": "Updated instructions" }.
    /// Note: Capacity goal is required and must be between 0.01 and 5000 mL.
    /// </summary>
    /// <param name="request">Spirometry setup update request.</param>
    /// <returns>Success response with spirometry setup ID.</returns>
    [HttpPut]
    [ProducesResponseType<SpirometrySetupResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutSpirometrySetup([FromBody] SpirometrySetupUpdateRequest request)
    {
        var setupId = await _spirometrySetupDataService.UpdateSpirometrySetupAsync(request);
        var response = new SpirometrySetupResponse
        {
            Id = setupId,
            Message = "Spirometry setup updated successfully",
        };

        _logger.LogDebug("Spirometry setup updated successfully with ID {SetupId}", setupId);
        return Ok(response);
    }

    /// <summary>
    /// Delete spirometry setup for a patient
    /// Example:
    /// - DELETE /api/v1/spirometry-setup/patient/123 (deletes spirometry setup for patient 123).
    /// </summary>
    /// <param name="patientId">Patient ID.</param>
    /// <returns>Success response.</returns>
    [HttpDelete("patient/{patientId}")]
    [ProducesResponseType<SpirometrySetupResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteSpirometrySetup([Range(1, int.MaxValue)] int patientId)
    {
        _logger.LogDebug("DeleteSpirometrySetup called for patient {PatientId}", patientId);

        var success = await _spirometrySetupDataService.DeleteSpirometrySetupAsync(patientId);

        var response = new SpirometrySetupResponse
        {
            Id = 0,
            Message = "Spirometry setup deleted successfully",
        };

        _logger.LogDebug("Spirometry setup deleted successfully for patient {PatientId}", patientId);
        return Ok(response);
    }
}
