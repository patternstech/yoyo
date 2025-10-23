namespace AH.CancerConnect.API.Features.SymptomsTracking.Models;

/// <summary>
/// Response model for symptom information.
/// </summary>
public class SymptomResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string DisplayTitle { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public List<string> AvailableValues { get; set; } = new List<string>();
}

/// <summary>
/// Response model for symptom entry creation.
/// </summary>
public class SymptomEntryResponse
{
    public bool Success { get; set; }

    public int EntryId { get; set; }

    public int Count { get; set; }

    public List<string>? ValidationErrors { get; set; }
}

/// <summary>
/// Response model for retrieving symptom entries with detailed information.
/// </summary>
public class SymptomEntryDetailResponse
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public string Note { get; set; } = string.Empty;

    public DateTime EntryDate { get; set; }

    public DateTime Created { get; set; }

    public string Summary { get; set; } = string.Empty;

    public List<SymptomDetailResponse> SymptomDetails { get; set; } = new List<SymptomDetailResponse>();
}

/// <summary>
/// Response model for individual symptom details.
/// </summary>
public class SymptomDetailResponse
{
    public int Id { get; set; }

    public int SymptomId { get; set; }

    public string SymptomName { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public string SymptomValue { get; set; } = string.Empty;
}