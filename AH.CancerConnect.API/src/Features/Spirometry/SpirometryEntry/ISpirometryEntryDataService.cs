namespace AH.CancerConnect.API.Features.Spirometry.SpirometryEntry;

/// <summary>
/// Interface for spirometry entry data service.
/// </summary>
public interface ISpirometryEntryDataService
{
    /// <summary>
    /// Creates a new spirometry entry.
    /// </summary>
    /// <param name="request">Spirometry entry request.</param>
    /// <returns>ID of the created spirometry entry.</returns>
    Task<int> CreateSpirometryEntryAsync(SpirometryEntryRequest request);

    /// <summary>
    /// Updates an existing spirometry entry.
    /// </summary>
    /// <param name="entryId">ID of the spirometry entry to update.</param>
    /// <param name="request">Updated spirometry entry data.</param>
    /// <returns>True if updated successfully.</returns>
    Task<bool> UpdateSpirometryEntryAsync(int entryId, SpirometryEntryUpdateRequest request);

    /// <summary>
    /// Gets spirometry graph data for a patient.
    /// </summary>
    /// <param name="request">Spirometry graph request with patient ID and days.</param>
    /// <returns>Spirometry graph data with capacity goal and daily values.</returns>
    Task<SpirometryGraphResponse> GetSpirometryGraphAsync(SpirometryGraphRequest request);

    /// <summary>
    /// Deletes a spirometry entry by ID.
    /// </summary>
    /// <param name="entryId">ID of the spirometry entry to delete.</param>
    /// <returns>True if deleted successfully.</returns>
    Task<bool> DeleteSpirometryEntryAsync(int entryId);

    /// <summary>
    /// Gets all spirometry entries for a patient.
    /// </summary>
    /// <param name="patientId">ID of the patient.</param>
    /// <returns>List of all spirometry entries for the patient.</returns>
    Task<IEnumerable<SpirometryEntryDetailResponse>> GetAllEntriesByPatientAsync(int patientId);
}
