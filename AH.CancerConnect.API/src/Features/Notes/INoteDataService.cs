namespace AH.CancerConnect.API.Features.Notes;

/// <summary>
/// Interface for note data service.
/// </summary>
public interface INoteDataService
{
    /// <summary>
    /// Creates a new note for a patient.
    /// </summary>
    /// <param name="request">Note request.</param>
    /// <returns>ID of the created note.</returns>
    Task<int> CreateNoteAsync(NoteRequest request);

    /// <summary>
    /// Updates an existing note.
    /// </summary>
    /// <param name="request">Update request.</param>
    /// <returns>ID of the updated note.</returns>
    Task<int> UpdateNoteAsync(NoteUpdateRequest request);

    /// <summary>
    /// Deletes a note.
    /// </summary>
    /// <param name="noteId">Note ID to delete.</param>
    /// <param name="patientId">Patient ID for validation.</param>
    /// <returns>True if deleted successfully.</returns>
    Task<bool> DeleteNoteAsync(int noteId, int patientId);

    /// <summary>
    /// Gets all notes for a patient.
    /// </summary>
    /// <param name="patientId">Patient ID.</param>
    /// <returns>List of notes.</returns>
    Task<IEnumerable<NoteDetailResponse>> GetNotesByPatientAsync(int patientId);
}
