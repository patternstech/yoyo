using Microsoft.EntityFrameworkCore;

namespace AH.CancerConnect.API.Features.Questions;

/// <summary>
/// Database implementation of question data service.
/// </summary>
public class QuestionDataService : IQuestionDataService
{
    private readonly CancerConnectDbContext _dbContext;
    private readonly ILogger<QuestionDataService> _logger;

    public QuestionDataService(CancerConnectDbContext dbContext, ILogger<QuestionDataService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<int> CreateQuestionAsync(QuestionRequest request)
    {
        _logger.LogDebug("Creating question for patient {PatientId}", request.PatientId);

        // Validate question text
        ValidateQuestionText(request.QuestionText);

        // Validate answer text
        ValidateAnswerText(request.AnswerText);

        // Create the question using extension method
        var question = request.ToEntity();

        // Save to database
        _dbContext.Questions.Add(question);
        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully created question {QuestionId} for patient {PatientId}", question.Id, request.PatientId);

        return question.Id;
    }

    /// <inheritdoc />
    public async Task<int> UpdateQuestionAsync(QuestionUpdateRequest request)
    {
        _logger.LogDebug("Updating question {QuestionId} for patient {PatientId}", request.Id, request.PatientId);

        // Validate question text
        ValidateQuestionText(request.QuestionText);

        // Validate answer text
        ValidateAnswerText(request.AnswerText);

        // Retrieve the existing question
        var question = await _dbContext.Questions
            .FirstOrDefaultAsync(n => n.Id == request.Id);

        if (question == null)
        {
            throw new KeyNotFoundException($"Question with ID {request.Id} not found.");
        }

        // Verify the question belongs to the specified patient
        if (question.PatientId != request.PatientId)
        {
            throw new ArgumentException($"Question {request.Id} does not belong to patient {request.PatientId}.");
        }

        // Update question properties using extension method
        question.UpdateFrom(request);

        // Save tracked changes
        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully updated question {QuestionId} for patient {PatientId}", question.Id, request.PatientId);

        return question.Id;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteQuestionAsync(int questionId, int patientId)
    {
        _logger.LogDebug("Deleting question {QuestionId} for patient {PatientId}", questionId, patientId);

        // Retrieve the existing question
        var question = await _dbContext.Questions
            .FirstOrDefaultAsync(n => n.Id == questionId);

        if (question == null)
        {
            throw new KeyNotFoundException($"Question with ID {questionId} not found.");
        }

        // Verify the question belongs to the specified patient
        if (question.PatientId != patientId)
        {
            throw new ArgumentException($"Question {questionId} does not belong to patient {patientId}.");
        }

        // Remove the question
        _dbContext.Questions.Remove(question);
        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully deleted question {QuestionId} for patient {PatientId}", questionId, patientId);

        return true;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<QuestionDetailResponse>> GetQuestionsByPatientAsync(int patientId)
    {
        _logger.LogDebug("Retrieving questions for patient {PatientId}", patientId);

        var questions = await _dbContext.Questions
            .Where(n => n.PatientId == patientId)
            .ToListAsync();

        _logger.LogDebug("Retrieved {Count} questions for patient {PatientId}", questions.Count, patientId);

        return questions.ToDetailResponses();
    }

    /// <summary>
    /// Validates question text length.
    /// </summary>
    /// <param name="questionText">The question text to validate.</param>
    private void ValidateQuestionText(string questionText)
    {
        if (string.IsNullOrWhiteSpace(questionText))
        {
            throw new ArgumentException("Question text cannot be empty.");
        }

        if (questionText.Length > 2000)
        {
            throw new ArgumentException($"Question text cannot exceed 2000 characters. Current length: {questionText.Length}");
        }
    }

    /// <summary>
    /// Validates answer text length.
    /// </summary>
    /// <param name="answerText">The answer text to validate.</param>
    private void ValidateAnswerText(string? answerText)
    {
        if (!string.IsNullOrWhiteSpace(answerText) && answerText.Length > 2000)
        {
            throw new ArgumentException($"Answer text cannot exceed 2000 characters. Current length: {answerText.Length}");
        }
    }
}