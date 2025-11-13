namespace AH.CancerConnect.API.Features.Spirometry.SpirometrySetup;

/// <summary>
/// Response DTO for spirometry setup operations.
/// </summary>
public class SpirometrySetupResponse
{
    public int Id { get; set; }

    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// Detailed response DTO for spirometry setup retrieval.
/// </summary>
public class SpirometrySetupDetailResponse
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public decimal? CapacityGoal { get; set; }

    public string? ProviderInstructions { get; set; }
}
