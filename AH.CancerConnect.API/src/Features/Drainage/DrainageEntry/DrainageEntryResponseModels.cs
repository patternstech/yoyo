namespace AH.CancerConnect.API.Features.Drainage.DrainageEntry;

/// <summary>
/// Response DTO for drainage entry creation operations.
/// </summary>
public class DrainageEntryResponse
{
    public List<int> EntryIds { get; set; } = new List<int>();

    public string Message { get; set; } = "Drainage entries created successfully";
}

/// <summary>
/// Response DTO for drainage entry update/delete operations.
/// </summary>
public class DrainageEntrySingleResponse
{
    public int Id { get; set; }

    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// Detailed response DTO for drainage entry.
/// </summary>
public class DrainageEntryDetailResponse
{
    public int Id { get; set; }

    public int DrainId { get; set; }

    public DateTime EmptyDate { get; set; }

    public decimal Amount { get; set; }

    public string? Note { get; set; }

    public bool IsArchived { get; set; }

    public DateTime DateCreated { get; set; }
}