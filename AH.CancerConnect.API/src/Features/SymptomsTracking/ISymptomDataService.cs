using AH.CancerConnect.API.Features.SymptomsTracking.Models;

namespace AH.CancerConnect.API.Features.SymptomsTracking;

/// <summary>
/// Interface for symptom data service.
/// </summary>
public interface ISymptomDataService
{
    /// <summary>
    /// Gets symptoms with severity options.
    /// </summary>
    /// <returns>List of symptoms with severity options.</returns>
    Task<IEnumerable<SymptomResponse>> GetSymptomsAsync();

    /// <summary>
    /// Creates a symptom entry for a patient.
    /// </summary>
    /// <param name="request">Entry request containing patient symptom details.</param>
    /// <returns>ID of the created entry.</returns>
    Task<int> CreateSymptomEntryAsync(SymptomEntryRequest request);

    /// <summary>
    /// Updates a symptom entry for a patient.
    /// </summary>
    /// <param name="request">Update request containing patient symptom details.</param>
    /// <returns>ID of the updated entry.</returns>
    Task<int> UpdateSymptomEntryAsync(SymptomEntryUpdateRequest request);

    /// <summary>
    /// Deletes a symptom entry and all its details.
    /// </summary>
    /// <param name="entryId">ID of the entry to delete.</param>
    /// <param name="patientId">Patient ID for validation.</param>
    /// <returns>True if deleted successfully.</returns>
    Task<bool> DeleteSymptomEntryAsync(int entryId, int patientId);

    /// <summary>
    /// Gets symptom summary for a patient containing all symptom entries and their details.
    /// </summary>
    /// <param name="patientId">Patient ID to retrieve symptom summary for.</param>
    /// <returns>List of all symptom entries with details for the patient.</returns>
    Task<IEnumerable<SymptomEntryDetailResponse>> GetSymptomSummaryAsync(int patientId);

    /// <summary>
    /// Gets symptom graph data for a patient within a date range.
    /// </summary>
    /// <param name="patientId">Patient ID to retrieve graph data for.</param>
    /// <param name="days">Number of days to look back from today.</param>
    /// <returns>Graph data for symptoms within the specified date range.</returns>
    Task<SymptomGraphResponse> GetSymptomGraphDataAsync(int patientId, int days);
}