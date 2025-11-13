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
/// Grouped drainage session response - represents one emptying session with multiple drains.
/// </summary>
public class DrainageSessionResponse
{
    public int DrainageEntryId { get; set; }

    public int PatientId { get; set; }

    public DateTime EmptyDate { get; set; }

    public List<DrainEntryDetail> DrainEntries { get; set; } = new List<DrainEntryDetail>();

    public string? Note { get; set; }
}

/// <summary>
/// Individual drain detail within a drainage session.
/// </summary>
public class DrainEntryDetail
{
    public int DrainId { get; set; }

    public decimal Amount { get; set; }

    public string? DrainName { get; set; }

    public bool IsArchived { get; set; }
}