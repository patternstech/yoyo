namespace AH.CancerConnect.API.Features.Spirometry.SpirometrySetup;

/// <summary>
/// Interface for spirometry setup data service.
/// </summary>
public interface ISpirometrySetupDataService
{
    /// <summary>
    /// Creates a new spirometry setup for a patient.
    /// </summary>
    /// <param name="request">Spirometry setup request.</param>
    /// <returns>ID of the created spirometry setup.</returns>
    Task<int> CreateSpirometrySetupAsync(SpirometrySetupRequest request);

    /// <summary>
    /// Gets the spirometry setup for a patient.
    /// </summary>
    /// <param name="patientId">Patient ID.</param>
    /// <returns>Spirometry setup details or null if not found.</returns>
    Task<SpirometrySetupDetailResponse?> GetSpirometrySetupByPatientAsync(int patientId);

    /// <summary>
    /// Updates an existing spirometry setup.
    /// </summary>
    /// <param name="request">Update request.</param>
    /// <returns>ID of the updated spirometry setup.</returns>
    Task<int> UpdateSpirometrySetupAsync(SpirometrySetupUpdateRequest request);

    /// <summary>
    /// Deletes a spirometry setup for a patient.
    /// </summary>
    /// <param name="patientId">Patient ID.</param>
    /// <returns>True if deleted successfully.</returns>
    Task<bool> DeleteSpirometrySetupAsync(int patientId);
}
