namespace AH.CancerConnect.API.Features.Drainage.DrainageEntry;

/// <summary>
/// Interface for drainage entry data service.
/// </summary>
public interface IDrainageEntryDataService
{
    /// <summary>
    /// Creates drainage entries for multiple drains.
    /// </summary>
    /// <param name="request">Drainage entry request with multiple drain entries.</param>
    /// <returns>List of created drainage entry IDs.</returns>
    Task<List<int>> CreateDrainageEntryAsync(DrainageEntryRequest request);

    /// <summary>
    /// Updates an existing drainage entry.
    /// </summary>
    /// <param name="entryId">ID of the drainage entry to update.</param>
    /// <param name="request">Updated drainage entry data.</param>
    /// <returns>True if updated successfully.</returns>
    Task<bool> UpdateDrainageEntryAsync(int entryId, DrainageEntryUpdateRequest request);

    /// <summary>
    /// Gets drainage sessions grouped by empty date for a patient.
    /// </summary>
    /// <param name="patientId">ID of the patient.</param>
    /// <returns>List of grouped drainage sessions.</returns>
    Task<IEnumerable<DrainageSessionResponse>> GetDrainageSessionsByPatientAsync(int patientId);

    /// <summary>
    /// Deletes a drainage entry by ID (archives it).
    /// </summary>
    /// <param name="entryId">ID of the drainage entry to delete.</param>
    /// <returns>True if deleted successfully.</returns>
    Task<bool> DeleteDrainageEntryAsync(int entryId);
}