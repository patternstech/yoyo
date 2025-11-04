using System.ComponentModel.DataAnnotations;
using AH.CancerConnect.API.SharedModels;

namespace AH.CancerConnect.API.Features.SymptomsTracking.Models;

// Main symptom entry record per patient
public class SymptomEntry
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public string? Note { get; set; }

    public DateTime EntryDate { get; set; }

    public DateTime Created { get; set; }

    // Navigation properties
    public Patient Patient { get; set; } = null!;

    public ICollection<SymptomDetail> SymptomDetails { get; set; } = new List<SymptomDetail>();
}

// Individual symptom details within an entry
public class SymptomDetail
{
    public int Id { get; set; }

    public int SymptomEntryId { get; set; }

    public int SymptomId { get; set; }

    public int CategoryId { get; set; }

    public string SymptomValue { get; set; } = string.Empty;

    // Navigation properties
    public SymptomEntry SymptomEntry { get; set; } = null!;

    public Symptom Symptom { get; set; } = null!;

    public SymptomCategory Category { get; set; } = null!;
}

// Request DTOs
public class SymptomEntryRequest
{
    [Required(ErrorMessage = "Patient ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
    public int PatientId { get; set; }

    [StringLength(2000, ErrorMessage = "Note cannot exceed 2000 characters.")]
    public string? Note { get; set; }

    [Required(ErrorMessage = "Entry date is required.")]
    public DateTime EntryDate { get; set; }

    [Required(ErrorMessage = "Symptom details are required.")]
    [MinLength(1, ErrorMessage = "At least one symptom detail must be provided.")]
    public List<SymptomDetailRequest> SymptomDetails { get; set; } = new List<SymptomDetailRequest>();
}

public class SymptomDetailRequest
{
    [Required(ErrorMessage = "Symptom ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Symptom ID must be a positive integer.")]
    public int SymptomId { get; set; }

    [Required(ErrorMessage = "Category ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Category ID must be a positive integer.")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Symptom value is required.")]
    [StringLength(100, ErrorMessage = "Symptom value cannot exceed 100 characters.")]
    public string SymptomValue { get; set; } = string.Empty;
}

public class SymptomEntryUpdateRequest
{
    [Required(ErrorMessage = "Entry ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Entry ID must be a positive integer.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Patient ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
    public int PatientId { get; set; }

    [StringLength(2000, ErrorMessage = "Note cannot exceed 2000 characters.")]
    public string? Note { get; set; }

    [Required(ErrorMessage = "Entry date is required.")]
    public DateTime EntryDate { get; set; }

    [Required(ErrorMessage = "Symptom details are required.")]
    [MinLength(1, ErrorMessage = "At least one symptom detail must be provided.")]
    public List<SymptomDetailUpdateRequest> SymptomDetails { get; set; } = new List<SymptomDetailUpdateRequest>();
}

public class SymptomDetailUpdateRequest
{
    public int? Id { get; set; } // Null for new details, populated for existing details

    [Required(ErrorMessage = "Symptom ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Symptom ID must be a positive integer.")]
    public int SymptomId { get; set; }

    [Required(ErrorMessage = "Category ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Category ID must be a positive integer.")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Symptom value is required.")]
    [StringLength(100, ErrorMessage = "Symptom value cannot exceed 100 characters.")]
    public string SymptomValue { get; set; } = string.Empty;
}