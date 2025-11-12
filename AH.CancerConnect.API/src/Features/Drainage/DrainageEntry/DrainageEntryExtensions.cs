namespace AH.CancerConnect.API.Features.Drainage.DrainageEntry;

/// <summary>
/// Extension methods for mapping drainage entry entities to responses.
/// </summary>
public static class DrainageEntryExtensions
{
    /// <summary>
    /// Converts a DrainageEntry entity to a DrainageEntryDetailResponse.
    /// </summary>
    /// <param name="entry">The drainage entry entity.</param>
    /// <returns>A DrainageEntryDetailResponse.</returns>
    public static DrainageEntryDetailResponse ToDetailResponse(this DrainageEntry entry)
    {
        return new DrainageEntryDetailResponse
        {
            Id = entry.Id,
            DrainId = entry.DrainId,
            EmptyDate = entry.EmptyDate,
            Amount = entry.Amount,
            Note = entry.Note,
            IsArchived = entry.IsArchived,
            DateCreated = entry.DateCreated,
        };
    }
}