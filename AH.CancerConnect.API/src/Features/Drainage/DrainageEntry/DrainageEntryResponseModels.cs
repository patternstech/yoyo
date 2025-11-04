namespace AH.CancerConnect.API.Features.Drainage.DrainageEntry;

/// <summary>
/// Response DTO for drainage entry operations.
/// </summary>
public class DrainageEntryResponse
{
    public int Id { get; set; }

    public string Message { get; set; } = "Drainage entry processed successfully";
}