namespace AH.CancerConnect.API.Features.SymptomsTracking.Models;

/// <summary>
/// Represents a symptom category (reference table).
/// </summary>
public class SymptomCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string DisplayValue { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<SymptomRange> SymptomRanges { get; set; } = new List<SymptomRange>();

    public ICollection<SymptomDetail> SymptomDetails { get; set; } = new List<SymptomDetail>();
}

/// <summary>
/// Represents a symptom (reference table).
/// </summary>
public class Symptom
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string DisplayTitle { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool Invalid { get; set; }

    // Navigation properties
    public ICollection<SymptomRange> SymptomRanges { get; set; } = new List<SymptomRange>();

    public ICollection<SymptomDetail> SymptomDetails { get; set; } = new List<SymptomDetail>();
}

/// <summary>
/// Represents symptom range values (reference table).
/// </summary>
public class SymptomRange
{
    public int Id { get; set; }

    public int SymptomId { get; set; }

    public int CategoryId { get; set; }

    public string SymptomValue { get; set; } = string.Empty;

    // Navigation properties
    public Symptom Symptom { get; set; } = null!;

    public SymptomCategory Category { get; set; } = null!;
}
