namespace AH.CancerConnect.API.Features.Questions;

/// <summary>
/// Interface for question data service.
/// </summary>
public interface IQuestionDataService
{
    /// <summary>
    /// Creates a new question for a patient.
    /// </summary>
    /// <param name="request">Question request.</param>
    /// <returns>ID of the created question.</returns>
    Task<int> CreateQuestionAsync(QuestionRequest request);

    /// <summary>
    /// Updates an existing question.
    /// </summary>
    /// <param name="request">Update request.</param>
    /// <returns>ID of the updated question.</returns>
    Task<int> UpdateQuestionAsync(QuestionUpdateRequest request);

    /// <summary>
    /// Deletes a question.
    /// </summary>
    /// <param name="questionId">Question ID to delete.</param>
    /// <param name="patientId">Patient ID for validation.</param>
    /// <returns>True if deleted successfully.</returns>
    Task<bool> DeleteQuestionAsync(int questionId, int patientId);

    /// <summary>
    /// Gets all questions for a patient.
    /// </summary>
    /// <param name="patientId">Patient ID.</param>
    /// <returns>List of questions.</returns>
    Task<IEnumerable<QuestionDetailResponse>> GetQuestionsByPatientAsync(int patientId);
}