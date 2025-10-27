namespace AH.CancerConnect.AdminAPI.Features.SymptomConfiguration;

/// <summary>
/// Response model for symptom configuration list/detail.
/// </summary>
public class SymptomConfigurationResponse
{
    public int Id { get; set; }

    public int SymptomId { get; set; }

    public string SymptomName { get; set; } = string.Empty;

    public string AlertTrigger { get; set; } = string.Empty;

    public bool FollowUp { get; set; }

    public string? Question { get; set; }

    public string Created { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}
