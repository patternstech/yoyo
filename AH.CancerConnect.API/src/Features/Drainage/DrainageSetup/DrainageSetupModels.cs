using System.ComponentModel.DataAnnotations;
using AH.CancerConnect.API.SharedModels;

namespace AH.CancerConnect.API.Features.Drainage.DrainageSetup;

/// <summary>
/// Main drainage setup entity for patient drainage tracking.
/// </summary>
public class DrainageSetup
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public bool HasProviderGoalAmount { get; set; }

    public int? GoalDrainageAmount { get; set; }

    public string? ProviderInstructions { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    // Navigation properties
    public Patient Patient { get; set; } = null!;

    public ICollection<Drain> Drains { get; set; } = new List<Drain>();
}

/// <summary>
/// Drain entity representing individual drains in a drainage setup.
/// </summary>
public class Drain
{
    public int Id { get; set; }

    public int DrainageSetupId { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsArchived { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DateArchived { get; set; }

    // Navigation property
    public DrainageSetup DrainageSetup { get; set; } = null!;
}

/// <summary>
/// Request DTO for creating a new drainage setup.
/// </summary>
public class DrainageSetupRequest
{
    [Required(ErrorMessage = "Patient ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
    public int PatientId { get; set; }

    [Required(ErrorMessage = "Provider goal amount selection is required.")]
    public bool HasProviderGoalAmount { get; set; }

    [Range(20, 50, ErrorMessage = "Goal drainage amount must be between 20 and 50 mL.")]
    public int? GoalDrainageAmount { get; set; }

    [StringLength(1000, ErrorMessage = "Provider instructions cannot exceed 1000 characters.")]
    public string? ProviderInstructions { get; set; }

    [Required(ErrorMessage = "At least one drain is required.")]
    [MinLength(1, ErrorMessage = "At least one drain is required.")]
    [MaxLength(4, ErrorMessage = "Maximum 4 drains are allowed.")]
    public List<DrainRequest> Drains { get; set; } = new List<DrainRequest>();
}

/// <summary>
/// Request DTO for drain information.
/// </summary>
public class DrainRequest
{
    public int? Id { get; set; } // Null for new drains

    [Required(ErrorMessage = "Drain name is required.")]
    [StringLength(20, ErrorMessage = "Drain name cannot exceed 20 characters.")]
    public string Name { get; set; } = string.Empty;

    public bool IsArchived { get; set; } = false;
}

/// <summary>
/// Request DTO for updating an existing drainage setup.
/// </summary>
public class DrainageSetupUpdateRequest
{
    [Required(ErrorMessage = "Drainage setup ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Drainage setup ID must be a positive integer.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Patient ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
    public int PatientId { get; set; }

    [Required(ErrorMessage = "Provider goal amount selection is required.")]
    public bool HasProviderGoalAmount { get; set; }

    [Range(20, 50, ErrorMessage = "Goal drainage amount must be between 20 and 50 mL.")]
    public int? GoalDrainageAmount { get; set; }

    [StringLength(1000, ErrorMessage = "Provider instructions cannot exceed 1000 characters.")]
    public string? ProviderInstructions { get; set; }

    [Required(ErrorMessage = "At least one drain is required.")]
    [MinLength(1, ErrorMessage = "At least one drain is required.")]
    [MaxLength(4, ErrorMessage = "Maximum 4 drains are allowed.")]
    public List<DrainRequest> Drains { get; set; } = new List<DrainRequest>();
}

/// <summary>
/// Request DTO for archiving a drain.
/// </summary>
public class ArchiveDrainRequest
{
    [Required(ErrorMessage = "Drain ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Drain ID must be a positive integer.")]
    public int DrainId { get; set; }

    [Required(ErrorMessage = "Patient ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
    public int PatientId { get; set; }
}