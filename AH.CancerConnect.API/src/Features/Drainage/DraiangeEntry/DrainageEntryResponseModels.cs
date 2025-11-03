namespace AH.CancerConnect.API.Features.Drainage.DraiangeEntry;

/// <summary>
/// Response DTO for drainage entry operations.
/// </summary>
public class DrainageEntryResponse
{
    public int Id { get; set; }

    public string Message { get; set; } = "Drainage entry processed successfully";
}

/// <summary>
/// Detailed response DTO for drainage entry retrieval.
/// </summary>
public class DrainageEntryDetailResponse
{
    public int Id { get; set; }

    public int DrainId { get; set; }

    public string DrainName { get; set; } = string.Empty;

    public DateTime EmptyDate { get; set; }

    public decimal? Drain1Amount { get; set; }

    public decimal? Drain2Amount { get; set; }

    public string? Note { get; set; }

    public bool IsArchived { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DateArchived { get; set; }
}