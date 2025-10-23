using System.ComponentModel.DataAnnotations;
using AH.CancerConnect.API.SharedModels;

namespace AH.CancerConnect.API.Features.Notes;

/// <summary>
/// Main note entity for patient notes.
/// </summary>
public class Note
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public string? Title { get; set; }

    public string NoteText { get; set; } = string.Empty;

    public string? RecordingPath { get; set; }

    public DateTime DateAdded { get; set; }

    public DateTime DateCreated { get; set; }

    // Navigation property
    public Patient Patient { get; set; } = null!;
}

/// <summary>
/// Request DTO for creating a new note.
/// </summary>
public class NoteRequest
{
    [Required(ErrorMessage = "Patient ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
    public int PatientId { get; set; }

    [StringLength(250, ErrorMessage = "Title cannot exceed 250 characters.")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Note text is required.")]
    [StringLength(1000, ErrorMessage = "Note text cannot exceed 1000 characters.")]
    public string NoteText { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Recording path cannot exceed 500 characters.")]
    public string? RecordingPath { get; set; }
}

/// <summary>
/// Request DTO for updating an existing note.
/// </summary>
public class NoteUpdateRequest
{
    [Required(ErrorMessage = "Note ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Note ID must be a positive integer.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Patient ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
    public int PatientId { get; set; }

    [StringLength(250, ErrorMessage = "Title cannot exceed 250 characters.")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Note text is required.")]
    [StringLength(1000, ErrorMessage = "Note text cannot exceed 1000 characters.")]
    public string NoteText { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Recording path cannot exceed 500 characters.")]
    public string? RecordingPath { get; set; }
}
