namespace AH.CancerConnect.API.Features.Spirometry.SpirometryEntry;

/// <summary>
/// Detailed response DTO for spirometry entry retrieval.
/// </summary>
public class SpirometryEntryDetailResponse
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public DateOnly TestDate { get; set; }

    public TimeOnly TestTime { get; set; }

    public decimal NumberReached { get; set; }

    public string? Note { get; set; }
}

/// <summary>
/// Response DTO for spirometry entry creation/update operations.
/// </summary>
public class SpirometryEntryResponse
{
    public int Id { get; set; }

    public string Message { get; set; } = string.Empty;
}
