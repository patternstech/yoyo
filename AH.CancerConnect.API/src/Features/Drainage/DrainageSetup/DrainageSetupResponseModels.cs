namespace AH.CancerConnect.API.Features.Drainage.DrainageSetup;

/// <summary>
/// Response DTO for drainage setup operations.
/// </summary>
public class DrainageSetupResponse
{
    public int Id { get; set; }

    public string Message { get; set; } = "Drainage setup processed successfully";
}

/// <summary>
/// Detailed response DTO for drainage setup retrieval.
/// </summary>
public class DrainageSetupDetailResponse
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public bool HasProviderGoalAmount { get; set; }

    public int? GoalDrainageAmount { get; set; }

    public string? ProviderInstructions { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    public List<DrainResponse> Drains { get; set; } = new List<DrainResponse>();
}

/// <summary>
/// Response DTO for drain information.
/// </summary>
public class DrainResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsArchived { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DateArchived { get; set; }
}

/// <summary>
/// Response DTO for drain archiving operations.
/// </summary>
public class ArchiveDrainResponse
{
    public int DrainId { get; set; }

    public string Message { get; set; } = "Drain archived successfully";
}