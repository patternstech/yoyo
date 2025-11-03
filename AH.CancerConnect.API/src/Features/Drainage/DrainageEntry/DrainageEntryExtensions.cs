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
            DrainageSetupId = request.DrainageSetupId,
            EmptyDate = request.EmptyDate,
            Drain1Amount = request.Drain1Amount,
            Drain2Amount = request.Drain2Amount,
            Drain3Amount = request.Drain3Amount,
            Drain4Amount = request.Drain4Amount,
            Note = request.Note,
            IsArchived = false,
            DateCreated = DateTime.UtcNow,
        };
    }
}