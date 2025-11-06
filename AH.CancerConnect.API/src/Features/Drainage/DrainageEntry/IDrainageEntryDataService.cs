namespace AH.CancerConnect.API.Features.Drainage.DrainageEntry;

/// <summary>
/// Interface for drainage entry data service.
/// </summary>
public interface IDrainageEntryDataService
{
    /// <summary>
    /// Creates a new drainage entry.
    /// </summary>
    /// <param name="request">Drainage entry request.</param>
    /// <returns>ID of the created drainage entry.</returns>
    Task<int> CreateDrainageEntryAsync(DrainageEntryRequest request);

    /// <summary>
    /// Updates an existing drainage entry.
    /// </summary>
    /// <param name="entryId">ID of the drainage entry to update.</param>
    /// <param name="request">Updated drainage entry data.</param>
    /// <returns>True if updated successfully.</returns>
    Task<bool> UpdateDrainageEntryAsync(int entryId, DrainageEntryUpdateRequest request);

    /// <summary>
    /// Gets a drainage entry by ID.
    /// </summary>
    /// <param name="entryId">ID of the drainage entry.</param>
    /// <returns>The drainage entry detail response.</returns>
    Task<DrainageEntryDetailResponse> GetDrainageEntryByIdAsync(int entryId);

    /// <summary>
    /// Deletes a drainage entry by ID (archives it).
    /// </summary>
    /// <param name="entryId">ID of the drainage entry to delete.</param>
    /// <returns>True if deleted successfully.</returns>
    Task<bool> DeleteDrainageEntryAsync(int entryId);
}