using System.ComponentModel.DataAnnotations;
using AH.CancerConnect.API.Features.Drainage.DrainageSetup;

namespace AH.CancerConnect.API.Features.Drainage.DrainageEntry;

/// <summary>
/// Main drainage entry entity for tracking individual drain measurements.
/// </summary>
public class DrainageEntry
{
    public int Id { get; set; }

    public int DrainId { get; set; }

    public DateTime EmptyDate { get; set; }

    public decimal Amount { get; set; }

    public string? Note { get; set; }

    public bool IsArchived { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DateArchived { get; set; }

    // Navigation property
    public Drain Drain { get; set; } = null!;
}

/// <summary>
/// Request DTO for creating a new drainage entry.
/// </summary>
public class DrainageEntryRequest
{
    [Required(ErrorMessage = "Drain ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Drain ID must be a positive integer.")]
    public int DrainId { get; set; }

    [Required(ErrorMessage = "Empty date is required.")]
    public DateTime EmptyDate { get; set; }

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0, 100, ErrorMessage = "Amount must be between 0 and 100 mL.")]
    public decimal Amount { get; set; }

    [StringLength(1000, ErrorMessage = "Note cannot exceed 1000 characters.")]
    public string? Note { get; set; }
}

/// <summary>
/// Request DTO for updating an existing drainage entry.
/// </summary>
public class DrainageEntryUpdateRequest
{
    [Required(ErrorMessage = "Empty date is required.")]
    public DateTime EmptyDate { get; set; }

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0, 100, ErrorMessage = "Amount must be between 0 and 100 mL.")]
    public decimal Amount { get; set; }

    [StringLength(1000, ErrorMessage = "Note cannot exceed 1000 characters.")]
    public string? Note { get; set; }
}