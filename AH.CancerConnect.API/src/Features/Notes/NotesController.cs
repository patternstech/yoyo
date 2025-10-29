namespace AH.CancerConnect.API.Features.Notes;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Notes Related Endpoints.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class NotesController : ControllerBase
{
    private readonly INoteDataService _noteDataService;
    private readonly ILogger<NotesController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotesController"/> class.
    /// </summary>
    /// <param name="noteDataService">Note data service.</param>
    /// <param name="logger">Logger instance.</param>
    public NotesController(INoteDataService noteDataService, ILogger<NotesController> logger)
    {
        _noteDataService = noteDataService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new note for a patient
    /// Example: POST /api/v1/notes
    /// Body: { "patientId": 123, "title": "Follow-up", "noteText": "Patient doing well", "recordingPath": "/recordings/file.mp3" }.
    /// </summary>
    /// <param name="request">Note request.</param>
    /// <returns>Success response with note ID.</returns>
    [HttpPost]
    [ProducesResponseType<NoteResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostNote([FromBody] NoteRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("PostNote called with invalid model state");
            return BadRequest(ModelState);
        }

        _logger.LogDebug("PostNote called for patient {PatientId}", request.PatientId);

        var noteId = await _noteDataService.CreateNoteAsync(request);

        return Ok(new NoteResponse
        {
            Success = true,
            Id = noteId,
        });
    }

    /// <summary>
    /// Update an existing note
    /// Example: PUT /api/v1/notes
    /// Body: { "id": 5, "patientId": 123, "title": "Updated Follow-up", "noteText": "Patient improving", "recordingPath": "/recordings/file2.mp3" }.
    /// </summary>
    /// <param name="request">Update request.</param>
    /// <returns>Success response with note ID.</returns>
    [HttpPut]
    [ProducesResponseType<NoteResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutNote([FromBody] NoteUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("PutNote called with invalid model state");
            return BadRequest(ModelState);
        }

        _logger.LogDebug("PutNote called for note {NoteId}", request.Id);

        var noteId = await _noteDataService.UpdateNoteAsync(request);

        return Ok(new NoteResponse
        {
            Success = true,
            Id = noteId,
        });
    }

    /// <summary>
    /// Delete a note
    /// Example: DELETE /api/v1/notes/5?patientId=123.
    /// </summary>
    /// <param name="id">Note ID to delete.</param>
    /// <param name="patientId">Patient ID for validation.</param>
    /// <returns>Success response.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType<NoteResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteNote(int id, [FromQuery][Required] int patientId)
    {
        _logger.LogDebug("DeleteNote called for note {NoteId} and patient {PatientId}", id, patientId);

        await _noteDataService.DeleteNoteAsync(id, patientId);

        return Ok(new NoteResponse
        {
            Success = true,
            Id = id,
        });
    }

    /// <summary>
    /// Get all notes for a patient
    /// Example: GET /api/v1/notes?patientId=123.
    /// </summary>
    /// <param name="patientId">Patient ID.</param>
    /// <returns>List of notes.</returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<NoteDetailResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetNotes([FromQuery][Required] int patientId)
    {
        _logger.LogDebug("GetNotes called for patient {PatientId}", patientId);

        var notes = await _noteDataService.GetNotesByPatientAsync(patientId);
        return Ok(notes);
    }
}
