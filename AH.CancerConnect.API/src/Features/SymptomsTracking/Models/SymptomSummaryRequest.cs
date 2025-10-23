using System.ComponentModel.DataAnnotations;

namespace AH.CancerConnect.API.Features.SymptomsTracking.Models;

/// <summary>
/// Request model for getting symptom summary.
/// </summary>
public class SymptomSummaryRequest
{
    /// <summary>
    /// Gets or sets the patient ID to retrieve symptom summary for.
    /// </summary>
    [Required(ErrorMessage = "Patient ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
    public int PatientId { get; set; }
}

/// <summary>
/// Request model for deleting symptom entry.
/// </summary>
public class DeleteSymptomEntryRequest
{
    /// <summary>
    /// Gets or sets the entry ID to delete.
    /// </summary>
    [Required(ErrorMessage = "Entry ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Entry ID must be a positive integer.")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the patient ID for validation.
    /// </summary>
    [Required(ErrorMessage = "Patient ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
    public int PatientId { get; set; }
}