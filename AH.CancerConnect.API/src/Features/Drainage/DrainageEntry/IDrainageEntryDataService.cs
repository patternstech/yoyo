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
    /// Updates multiple drainage entries in a session.
    /// </summary>
    /// <param name="request">Updated drainage entry data with multiple drain entries.</param>
    /// <returns>True if updated successfully.</returns>
    Task<bool> UpdateDrainageEntryAsync(DrainageEntryUpdateRequest request);

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

    /// <summary>
    /// Gets drainage graph data for a patient within a date range.
    /// </summary>
    /// <param name="request">Drainage graph request with patient ID and date range.</param>
    /// <returns>Drainage graph response with historical data and today's entries.</returns>
    Task<DrainageGraphResponse> GetDrainageGraphAsync(DrainageGraphRequest request);
}