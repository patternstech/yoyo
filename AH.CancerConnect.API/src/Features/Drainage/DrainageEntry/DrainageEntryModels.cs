using System.ComponentModel.DataAnnotations;
using AH.CancerConnect.API.Features.Drainage.DrainageSetup;

namespace AH.CancerConnect.API.Features.Drainage.DrainageEntry;

/// <summary>
/// Main drainage entry entity for tracking drain measurements for all drains at once.
/// </summary>
public class DrainageEntry
{
    public int Id { get; set; }

    public int DrainageSetupId { get; set; }

    public DateTime EmptyDate { get; set; }

    public decimal? Drain1Amount { get; set; }

    public decimal? Drain2Amount { get; set; }

    public decimal? Drain3Amount { get; set; }

    public decimal? Drain4Amount { get; set; }

    public string? Note { get; set; }

    public bool IsArchived { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DateArchived { get; set; }

    // Navigation property
    public DrainageSetup DrainageSetup { get; set; } = null!;
}

/// <summary>
/// Request DTO for creating a new drainage entry.
/// </summary>
public class DrainageEntryRequest
{
    [Required(ErrorMessage = "Drainage setup ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Drainage setup ID must be a positive integer.")]
    public int DrainageSetupId { get; set; }

    [Required(ErrorMessage = "Empty date is required.")]
    public DateTime EmptyDate { get; set; }

    [Range(0, 9999.99, ErrorMessage = "Drain 1 amount must be between 0 and 9999.99 mL.")]
    public decimal? Drain1Amount { get; set; }

    [Range(0, 9999.99, ErrorMessage = "Drain 2 amount must be between 0 and 9999.99 mL.")]
    public decimal? Drain2Amount { get; set; }

    [Range(0, 9999.99, ErrorMessage = "Drain 3 amount must be between 0 and 9999.99 mL.")]
    public decimal? Drain3Amount { get; set; }

    [Range(0, 9999.99, ErrorMessage = "Drain 4 amount must be between 0 and 9999.99 mL.")]
    public decimal? Drain4Amount { get; set; }

    [StringLength(1000, ErrorMessage = "Note cannot exceed 1000 characters.")]
    public string? Note { get; set; }
}