namespace AH.CancerConnect.API.Features.Spirometry.SpirometryEntry;

/// <summary>
/// Extension methods for SpirometryEntry entity and related models.
/// </summary>
public static class SpirometryEntryExtensions
{
    /// <summary>
    /// Converts a SpirometryEntryRequest to a SpirometryEntry entity.
    /// </summary>
    /// <param name="request">The spirometry entry request.</param>
    /// <returns>A new SpirometryEntry entity.</returns>
    public static SpirometryEntry ToEntity(this SpirometryEntryRequest request)
    {
        return new SpirometryEntry
        {
            PatientId = request.PatientId,
            TestDate = request.TestDate,
            TestTime = request.TestTime,
            NumberReached = request.NumberReached,
            Note = request.Note,
        };
    }

    /// <summary>
    /// Converts a SpirometryEntry entity to SpirometryEntryDetailResponse.
    /// </summary>
    /// <param name="entry">The spirometry entry entity.</param>
    /// <returns>A SpirometryEntryDetailResponse object.</returns>
    public static SpirometryEntryDetailResponse ToDetailResponse(this SpirometryEntry entry)
    {
        return new SpirometryEntryDetailResponse
        {
            Id = entry.Id,
            PatientId = entry.PatientId,
            TestDate = entry.TestDate,
            TestTime = entry.TestTime,
            NumberReached = entry.NumberReached,
            Note = entry.Note,
        };
    }

    /// <summary>
    /// Updates an existing SpirometryEntry entity from a SpirometryEntryUpdateRequest.
    /// </summary>
    /// <param name="entry">The existing spirometry entry entity.</param>
    /// <param name="request">The update request.</param>
    public static void UpdateFrom(this SpirometryEntry entry, SpirometryEntryUpdateRequest request)
    {
        entry.TestDate = request.TestDate;
        entry.TestTime = request.TestTime;
        entry.NumberReached = request.NumberReached;
        entry.Note = request.Note;
    }
}
