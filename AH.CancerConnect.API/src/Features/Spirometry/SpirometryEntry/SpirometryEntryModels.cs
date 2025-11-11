using System.ComponentModel.DataAnnotations;
using AH.CancerConnect.API.SharedModels;

namespace AH.CancerConnect.API.Features.Spirometry.SpirometryEntry;

/// <summary>
/// Main spirometry entry entity for tracking individual spirometry test results.
/// </summary>
public class SpirometryEntry
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public DateOnly TestDate { get; set; }

    public TimeOnly TestTime { get; set; }

    public decimal NumberReached { get; set; }

    public string? Note { get; set; }

    // Navigation property
    public Patient Patient { get; set; } = null!;
}

/// <summary>
/// Request DTO for creating a new spirometry entry.
/// </summary>
public class SpirometryEntryRequest
{
    [Required(ErrorMessage = "Patient ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
    public int PatientId { get; set; }

    [Required(ErrorMessage = "Test date is required.")]
    public DateOnly TestDate { get; set; }

    [Required(ErrorMessage = "Test time is required.")]
    public TimeOnly TestTime { get; set; }

    [Required(ErrorMessage = "Number reached is required.")]
    [Range(0.01, 10000, ErrorMessage = "Number reached must be between 0.01 and 10000 mL.")]
    public decimal NumberReached { get; set; }

    [StringLength(1000, ErrorMessage = "Note cannot exceed 1000 characters.")]
    public string? Note { get; set; }
}

/// <summary>
/// Request DTO for updating an existing spirometry entry.
/// </summary>
public class SpirometryEntryUpdateRequest
{
    [Required(ErrorMessage = "Test date is required.")]
    public DateOnly TestDate { get; set; }

    [Required(ErrorMessage = "Test time is required.")]
    public TimeOnly TestTime { get; set; }

    [Required(ErrorMessage = "Number reached is required.")]
    [Range(0.01, 10000, ErrorMessage = "Number reached must be between 0.01 and 10000 mL.")]
    public decimal NumberReached { get; set; }

    [StringLength(1000, ErrorMessage = "Note cannot exceed 1000 characters.")]
    public string? Note { get; set; }
}
