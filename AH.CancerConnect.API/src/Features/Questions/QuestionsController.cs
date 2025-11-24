namespace AH.CancerConnect.API.Features.Questions;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Questions Related Endpoints.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionDataService _questionDataService;
    private readonly ILogger<QuestionsController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestionsController"/> class.
    /// </summary>
    /// <param name="questionDataService">Question data service.</param>
    /// <param name="logger">Logger instance.</param>
    public QuestionsController(IQuestionDataService questionDataService, ILogger<QuestionsController> logger)
    {
        _questionDataService = questionDataService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new question for a patient
    /// Example: POST /api/v1/questions
    /// Body: { "patientId": 123, "questionText": "how long it will take to cure", "answerText": "3 days",  }.
    /// </summary>
    /// <param name="request">Question request.</param>
    /// <returns>Success response with question ID.</returns>
    [HttpPost]
    [ProducesResponseType<QuestionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostQuestion([FromBody] QuestionRequest request)
    {
        _logger.LogDebug("PostQuestion called for patient {PatientId}", request.PatientId);

        var questionId = await _questionDataService.CreateQuestionAsync(request);

        return Ok(new QuestionResponse
        {
            Success = true,
            Id = questionId,
        });
    }

    /// <summary>
    /// Update an existing question
    /// Example: PUT /api/v1/questions
    /// Body: { "id": 5, "patientId": 123, "questionText": "how long it will take to cure", "answerText": "3 days" }.
    /// </summary>
    /// <param name="request">Update request.</param>
    /// <returns>Success response with question ID.</returns>
    [HttpPut]
    [ProducesResponseType<QuestionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutQuestion([FromBody] QuestionUpdateRequest request)
    {
        _logger.LogDebug("PutQuestion called for question {QuestionId}", request.Id);

        var questionId = await _questionDataService.UpdateQuestionAsync(request);

        return Ok(new QuestionResponse
        {
            Success = true,
            Id = questionId,
        });
    }

    /// <summary>
    /// Delete a question
    /// Example: DELETE /api/v1/questions/5?patientId=123.
    /// </summary>
    /// <param name="id">Question ID to delete.</param>
    /// <param name="patientId">Patient ID for validation.</param>
    /// <returns>Success response.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType<QuestionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteQuestion(int id, [FromQuery][Required] int patientId)
    {
        _logger.LogDebug("DeleteQuestion called for question {QuestionId} and patient {PatientId}", id, patientId);

        await _questionDataService.DeleteQuestionAsync(id, patientId);

        return Ok(new QuestionResponse
        {
            Success = true,
            Id = id,
        });
    }

    /// <summary>
    /// Get all questions for a patient
    /// Example: GET /api/v1/questions?patientId=123.
    /// </summary>
    /// <param name="patientId">Patient ID.</param>
    /// <returns>List of questions.</returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<QuestionDetailResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetQuestions([FromQuery][Required] int patientId)
    {
        _logger.LogDebug("GetQuestions called for patient {PatientId}", patientId);

        var questions = await _questionDataService.GetQuestionsByPatientAsync(patientId);
        return Ok(questions);
    }
}