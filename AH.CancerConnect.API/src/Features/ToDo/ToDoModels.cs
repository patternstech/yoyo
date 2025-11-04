using System.ComponentModel.DataAnnotations;
using AH.CancerConnect.API.SharedModels;

namespace AH.CancerConnect.API.Features.ToDo;

/// <summary>
/// Alert type enum for ToDo items.
/// </summary>
public enum AlertType
{
    /// <summary>
    /// NONE
    /// </summary>
    NONE,

    /// <summary>
    /// DAY_OF_EVENT
    /// </summary>
    DAY_OF_EVENT,

    /// <summary>
    /// EVENING_BEFORE_DAY_OF_EVENT
    /// </summary>
    EVENING_BEFORE_DAY_OF_EVENT,

    /// <summary>
    /// MORNING_BEFORE_DAY_OF_EVENT
    /// </summary>
    MORNING_BEFORE_DAY_OF_EVENT,

    /// <summary>
    /// TIME_OF_EVENT
    /// </summary>
    TIME_OF_EVENT,

    /// <summary>
    /// HOUR_BEFORE_EVENT
    /// </summary>
    HOUR_BEFORE_EVENT,

    /// <summary>
    /// DAY_BEFORE_EVENT
    /// </summary>
    DAY_BEFORE_EVENT,
}

/// <summary>
/// Main ToDo entity for patient tasks.
/// </summary>
public class ToDo
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Detail { get; set; }

    public DateOnly? Date { get; set; }

    public TimeSpan? Time { get; set; }

    public AlertType? Alert { get; set; }

    public bool IsCompleted { get; set; } = false;

    public DateTime DateCreated { get; set; }

    // Navigation property
    public Patient Patient { get; set; } = null!;
}

/// <summary>
/// Request DTO for creating a new ToDo item.
/// </summary>
public class ToDoRequest
{
    [Required(ErrorMessage = "Patient ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
    public int PatientId { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(250, ErrorMessage = "Title cannot exceed 250 characters.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Detail cannot exceed 1000 characters.")]
    public string? Detail { get; set; }

    public DateOnly? Date { get; set; }

    public TimeSpan? Time { get; set; }

    public AlertType? Alert { get; set; }
}
