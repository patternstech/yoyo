namespace AH.CancerConnect.AdminAPI.Features.SymptomConfiguration;

/// <summary>
/// Symptom configuration entity for managing symptom alert rules.
/// </summary>
public class SymptomConfiguration
{
    public int Id { get; set; }

    public int SymptomId { get; set; }

    public string AlertTrigger { get; set; } = string.Empty;

    public bool FollowUp { get; set; }

    public string? Question { get; set; }

    public string Created { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    // Navigation property
    public Symptom? Symptom { get; set; }
}

/// <summary>
/// Symptom reference model (from API project).
/// </summary>
public class Symptom
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string DisplayTitle { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool Invalid { get; set; }

    // Navigation property
    public ICollection<SymptomConfiguration> SymptomConfigurations { get; set; } = new List<SymptomConfiguration>();
}
