using System.ComponentModel.DataAnnotations;
using AH.CancerConnect.API.SharedModels;

namespace AH.CancerConnect.API.Features.Questions;

/// <summary>
/// Main Question entity for patient Questions.
/// </summary>
public class Question
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public string QuestionText { get; set; } = string.Empty;

    public string? AnswerText { get; set; }

    public DateTime DateCreated { get; set; }

    // Navigation property
    public Patient Patient { get; set; } = null!;
}

/// <summary>
/// Request DTO for creating a new Question.
/// </summary>
public class QuestionRequest
{
    [Required(ErrorMessage = "Patient ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
    public int PatientId { get; set; }

    [Required(ErrorMessage = "Question text is required.")]
    [StringLength(2000, ErrorMessage = "Question text cannot exceed 2000 characters.")]
    public string QuestionText { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Answer text cannot exceed 2000 characters.")]
    public string? AnswerText { get; set; }
}

/// <summary>
/// Request DTO for updating an existing Question.
/// </summary>
public class QuestionUpdateRequest
{
    [Required(ErrorMessage = "Question ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Question ID must be a positive integer.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Patient ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
    public int PatientId { get; set; }

    [Required(ErrorMessage = "Question text is required.")]
    [StringLength(2000, ErrorMessage = "Question text cannot exceed 2000 characters.")]
    public string QuestionText { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Answer text cannot exceed 2000 characters.")]
    public string? AnswerText { get; set; }
}