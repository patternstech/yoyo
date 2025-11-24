namespace AH.CancerConnect.API.Features.Questions;

/// <summary>
/// Response model for Question operations.
/// </summary>
public class QuestionResponse
{
    public bool Success { get; set; }

    public int Id { get; set; }

    public List<string>? ValidationErrors { get; set; }
}

/// <summary>
/// Response model for Question details.
/// </summary>
public class QuestionDetailResponse
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public string QuestionText { get; set; } = string.Empty;

    public string? AnswerText { get; set; }

    public DateTime DateCreated { get; set; }
}