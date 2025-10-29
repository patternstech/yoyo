using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AH.CancerConnect.API.Features.ToDo;

/// <summary>
/// Controller for managing patient ToDo items.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class ToDoController : ControllerBase
{
    private readonly IToDoDataService _todoDataService;
    private readonly ILogger<ToDoController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToDoController"/> class.
    /// </summary>
    /// <param name="todoDataService">ToDo data service.</param>
    /// <param name="logger">Logger instance.</param>
    public ToDoController(IToDoDataService todoDataService, ILogger<ToDoController> logger)
    {
        _todoDataService = todoDataService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new ToDo for a patient
    /// Example: POST /api/v1/todo
    /// Body: { "patientId": 123, "title": "Take Medicine", "detail": "Take blood pressure medication", "date": "2025-10-02", "time": "08:00:00", "alert": "OnDay" }.
    /// </summary>
    /// <param name="request">ToDo request.</param>
    /// <returns>Success response with ToDo ID.</returns>
    [HttpPost]
    [ProducesResponseType<ToDoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostToDo([FromBody] ToDoRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("PostToDo called with invalid model state");
            return BadRequest(ModelState);
        }

        _logger.LogDebug("PostToDo called for patient {PatientId}", request.PatientId);

        var todoId = await _todoDataService.CreateToDoAsync(request);

        return Ok(new ToDoResponse
        {
            Success = true,
            Id = todoId,
        });
    }

    /// <summary>
    /// Toggle completion status of a ToDo
    /// Example: PATCH /api/v1/todo/5/toggle?patientId=123.
    /// </summary>
    /// <param name="id">ToDo ID.</param>
    /// <param name="patientId">Patient ID for validation.</param>
    /// <returns>Success response.</returns>
    [HttpPatch("{id}/toggle")]
    [ProducesResponseType<ToDoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ToggleCompletion(int id, [FromQuery][Required] int patientId)
    {
        _logger.LogDebug("ToggleCompletion called for ToDo {ToDoId} and patient {PatientId}", id, patientId);

        await _todoDataService.ToggleCompletionAsync(id, patientId);

        return Ok(new ToDoResponse
        {
            Success = true,
            Id = id,
        });
    }

    /// <summary>
    /// Delete a ToDo
    /// Example: DELETE /api/v1/todo/5?patientId=123.
    /// </summary>
    /// <param name="id">ToDo ID to delete.</param>
    /// <param name="patientId">Patient ID for validation.</param>
    /// <returns>Success response.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType<ToDoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteToDo(int id, [FromQuery][Required] int patientId)
    {
        _logger.LogDebug("DeleteToDo called for ToDo {ToDoId} and patient {PatientId}", id, patientId);

        await _todoDataService.DeleteToDoAsync(id, patientId);

        return Ok(new ToDoResponse
        {
            Success = true,
            Id = id,
        });
    }

    /// <summary>
    /// Get all ToDos for a patient
    /// Example: GET /api/v1/todo?patientId=123.
    /// </summary>
    /// <param name="patientId">Patient ID.</param>
    /// <returns>List of ToDos.</returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<ToDoDetailResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetToDos([FromQuery][Required] int patientId)
    {
        _logger.LogDebug("GetToDos called for patient {PatientId}", patientId);

        var todos = await _todoDataService.GetToDosByPatientAsync(patientId);
        return Ok(todos);
    }
}
