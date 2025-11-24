namespace AH.CancerConnect.API.Features.Questions;

/// <summary>
/// Extension methods for mapping Question requests to entities.
/// </summary>
public static class QuestionExtensions
{
    /// <summary>
    /// Converts a QuestionRequest to a Question entity.
    /// </summary>
    /// <param name="request">The Question request.</param>
    /// <returns>A new Question entity.</returns>
    public static Question ToEntity(this QuestionRequest request)
    {
        return new Question
        {
            PatientId = request.PatientId,
            QuestionText = request.QuestionText,
            AnswerText = request.AnswerText,
            DateCreated = DateTime.Now,
        };
    }

    /// <summary>
    /// Updates an existing Question entity from a QuestionUpdateRequest.
    /// </summary>
    /// <param name="question">The existing Question entity.</param>
    /// <param name="request">The update request.</param>
    public static void UpdateFrom(this Question question, QuestionUpdateRequest request)
    {
        question.QuestionText = request.QuestionText;
        question.AnswerText = request.AnswerText;
    }

    /// <summary>
    /// Converts a Question entity to QuestionDetailResponse.
    /// </summary>
    /// <param name="question">The Question entity.</param>
    /// <returns>A QuestionDetailResponse object.</returns>
    public static QuestionDetailResponse ToResponse(this Question question)
    {
        return new QuestionDetailResponse
        {
            Id = question.Id,
            PatientId = question.PatientId,
            QuestionText = question.QuestionText,
            AnswerText = question.AnswerText,
            DateCreated = question.DateCreated,
        };
    }

    /// <summary>
    /// Converts a list of Question entities to QuestionDetailResponse objects.
    /// </summary>
    /// <param name="questions">The Questions to convert.</param>
    /// <returns>A list of QuestionDetailResponse objects.</returns>
    public static List<QuestionDetailResponse> ToDetailResponses(this IEnumerable<Question> questions)
    {
        return questions.Select(n => n.ToResponse()).ToList();
    }
}