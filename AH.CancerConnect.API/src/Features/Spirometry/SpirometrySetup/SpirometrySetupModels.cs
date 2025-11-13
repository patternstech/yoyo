using System.ComponentModel.DataAnnotations;
using AH.CancerConnect.API.SharedModels;

namespace AH.CancerConnect.API.Features.Spirometry.SpirometrySetup;

/// <summary>
/// Main spirometry setup entity for patient spirometry tracking.
/// </summary>
public class SpirometrySetup
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public decimal? CapacityGoal { get; set; }

    public string? ProviderInstructions { get; set; }

    // Navigation property
    public Patient Patient { get; set; } = null!;
}

/// <summary>
/// Request DTO for creating a new spirometry setup.
/// </summary>
public class SpirometrySetupRequest
{
    [Required(ErrorMessage = "Patient ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
    public int PatientId { get; set; }

    [Range(1, 5000, ErrorMessage = "Capacity goal must be between 1 and 5000 mL.")]
    public decimal? CapacityGoal { get; set; }

    [StringLength(1000, ErrorMessage = "Provider instructions cannot exceed 1000 characters.")]
    public string? ProviderInstructions { get; set; }
}

/// <summary>
/// Request DTO for updating an existing spirometry setup.
/// </summary>
public class SpirometrySetupUpdateRequest
{
    [Required(ErrorMessage = "Spirometry setup ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Spirometry setup ID must be a positive integer.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Patient ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
    public int PatientId { get; set; }

    [Range(1, 5000, ErrorMessage = "Capacity goal must be between 1 and 5000 mL.")]
    public decimal? CapacityGoal { get; set; }

    [StringLength(1000, ErrorMessage = "Provider instructions cannot exceed 1000 characters.")]
    public string? ProviderInstructions { get; set; }
}
