using System.ComponentModel.DataAnnotations;

namespace AH.CancerConnect.API.Features.Drainage.DrainageEntry;

/// <summary>
/// Request model for drainage graph data.
/// Returns all drainage data for the patient excluding today's entries.
/// </summary>
public class DrainageGraphRequest
{
    [Required(ErrorMessage = "Patient ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
    public int PatientId { get; set; }
}

/// <summary>
/// Response model for drainage graph data.
/// </summary>
public class DrainageGraphResponse
{
    public int TotalEntries { get; set; }

    public DrainageAlert Alert { get; set; }

    public List<DrainageDataPoint> DrainagesData { get; set; } = new List<DrainageDataPoint>();

    /// <summary>
    /// Today's drainage entries - always empty as today's data is excluded from the graph.
    /// </summary>
    public List<DrainageSessionResponse> TodayDrainageEntries { get; set; } = new List<DrainageSessionResponse>();
}

/// <summary>
/// Individual data point for drainage graph.
/// </summary>
public class DrainageDataPoint
{
    public DateOnly Date { get; set; }

    public decimal Value { get; set; }
}

/// <summary>
/// Alert level based on drainage amounts.
/// </summary>
public enum DrainageAlert
{
    /// <summary>
    /// No alert - normal drainage levels.
    /// </summary>
    NONE = 0,

    /// <summary>
    /// Drainage increased for two consecutive days.
    /// </summary>
    TWO_CONSECUTIVE_DAYS_INCREASED = 1,

    /// <summary>
    /// Drainage increased more than 50 mL in a single day.
    /// </summary>
    LARGE_INCREASE = 2,

    /// <summary>
    /// Patient reached their drainage goal.
    /// </summary>
    GOAL_REACHED = 3,
}
