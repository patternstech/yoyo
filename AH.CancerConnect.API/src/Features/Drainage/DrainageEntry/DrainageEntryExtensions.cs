namespace AH.CancerConnect.API.Features.Drainage.DrainageEntry;

/// <summary>
/// Extension methods for mapping drainage entry requests to entities.
/// </summary>
public static class DrainageEntryExtensions
{
    /// <summary>
    /// Converts a DrainageEntryRequest to a DrainageEntry entity.
    /// </summary>
    /// <param name="request">The drainage entry request.</param>
    /// <returns>A new DrainageEntry entity.</returns>
    public static DrainageEntry ToEntity(this DrainageEntryRequest request)
    {
        return new DrainageEntry
        {
            DrainId = request.DrainId,
            EmptyDate = request.EmptyDate,
            Amount = request.Amount,
            Note = request.Note,
            IsArchived = false,
            DateCreated = DateTime.UtcNow,
        };
    }

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