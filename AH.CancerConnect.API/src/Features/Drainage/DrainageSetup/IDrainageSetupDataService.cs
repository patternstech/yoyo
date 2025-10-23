namespace AH.CancerConnect.API.Features.Drainage.DrainageSetup;

/// <summary>
/// Interface for drainage setup data service.
/// </summary>
public interface IDrainageSetupDataService
{
    /// <summary>
    /// Creates a new drainage setup for a patient.
    /// </summary>
    /// <param name="request">Drainage setup request.</param>
    /// <returns>ID of the created drainage setup.</returns>
    Task<int> CreateDrainageSetupAsync(DrainageSetupRequest request);

    /// <summary>
    /// Gets the drainage setup for a patient.
    /// </summary>
    /// <param name="patientId">Patient ID.</param>
    /// <returns>Drainage setup details or null if not found.</returns>
    Task<DrainageSetupDetailResponse?> GetDrainageSetupByPatientAsync(int patientId);

    /// <summary>
    /// Updates an existing drainage setup.
    /// </summary>
    /// <param name="request">Update request.</param>
    /// <returns>ID of the updated drainage setup.</returns>
    Task<int> UpdateDrainageSetupAsync(DrainageSetupUpdateRequest request);

    /// <summary>
    /// Archives a drain and all its related entries.
    /// </summary>
    /// <param name="request">Archive drain request.</param>
    /// <returns>True if archived successfully.</returns>
    Task<bool> ArchiveDrainAsync(ArchiveDrainRequest request);
}