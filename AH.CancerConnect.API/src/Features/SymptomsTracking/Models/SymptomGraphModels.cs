using System.ComponentModel.DataAnnotations;

namespace AH.CancerConnect.API.Features.SymptomsTracking.Models;

/// <summary>
/// Request model for symptom graph data.
/// </summary>
public class SymptomGraphRequest
{
    [Required(ErrorMessage = "Patient ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
    public int PatientId { get; set; }

    [Required(ErrorMessage = "Days parameter is required.")]
    [Range(1, 365, ErrorMessage = "Days must be between 1 and 365.")]
    public int Days { get; set; } = 7;
}

/// <summary>
/// Response model for symptom graph data.
/// </summary>
public class SymptomGraphResponse
{
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int DaysWithSymptoms { get; set; }

    public int SymptomsTracked { get; set; }

    public List<SymptomGraphData> SymptomsData { get; set; } = new List<SymptomGraphData>();
}

/// <summary>
/// Individual symptom graph data.
/// </summary>
public class SymptomGraphData
{
    public string Name { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public List<SymptomValuePoint> Values { get; set; } = new List<SymptomValuePoint>();
}

/// <summary>
/// Individual data point for symptom value on a specific date.
/// </summary>
public class SymptomValuePoint
{
    public DateTime Date { get; set; }

    public string Value { get; set; } = string.Empty;
}