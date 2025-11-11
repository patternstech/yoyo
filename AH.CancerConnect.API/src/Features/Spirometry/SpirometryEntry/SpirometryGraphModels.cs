using System.ComponentModel.DataAnnotations;

namespace AH.CancerConnect.API.Features.Spirometry.SpirometryEntry;

/// <summary>
/// Request model for spirometry graph data.
/// </summary>
public class SpirometryGraphRequest
{
    [Required(ErrorMessage = "Patient ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
    public int PatientId { get; set; }

    [Required(ErrorMessage = "Days parameter is required.")]
    [Range(1, 365, ErrorMessage = "Days must be between 1 and 365.")]
    public int Days { get; set; } = 7;
}

/// <summary>
/// Response model for spirometry graph data.
/// </summary>
public class SpirometryGraphResponse
{
    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public decimal CapacityGoal { get; set; }

    public string? ProviderInstructions { get; set; }

    public int TotalEntries { get; set; }

    public List<SpirometryValuePoint> Values { get; set; } = new List<SpirometryValuePoint>();

    public List<SpirometryEntryDetailResponse> TodaysEntries { get; set; } = new List<SpirometryEntryDetailResponse>();
}

/// <summary>
/// Individual data point for spirometry value on a specific date.
/// </summary>
public class SpirometryValuePoint
{
    public DateOnly Date { get; set; }

    public decimal NumberReached { get; set; }
}
