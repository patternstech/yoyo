namespace AH.CancerConnect.API.Features.Notes;

/// <summary>
/// Extension methods for mapping note requests to entities.
/// </summary>
public static class NoteExtensions
{
    /// <summary>
    /// Converts a NoteRequest to a Note entity.
    /// </summary>
    /// <param name="request">The note request.</param>
    /// <returns>A new Note entity.</returns>
    public static Note ToEntity(this NoteRequest request)
    {
        return new Note
        {
            PatientId = request.PatientId,
            Title = request.Title,
            NoteText = request.NoteText,
        };
    }

    /// <summary>
    /// Updates an existing Note entity from a NoteUpdateRequest.
    /// </summary>
    /// <param name="note">The existing note entity.</param>
    /// <param name="request">The update request.</param>
    public static void UpdateFrom(this Note note, NoteUpdateRequest request)
    {
        note.Title = request.Title;
        note.NoteText = request.NoteText;
    }

    /// <summary>
    /// Converts a Note entity to NoteDetailResponse.
    /// </summary>
    /// <param name="note">The note entity.</param>
    /// <returns>A NoteDetailResponse object.</returns>
    public static NoteDetailResponse ToResponse(this Note note)
    {
        return new NoteDetailResponse
        {
            Id = note.Id,
            PatientId = note.PatientId,
            Title = note.Title,
            NoteText = note.NoteText
        };
    }

    /// <summary>
    /// Converts a list of Note entities to NoteDetailResponse objects.
    /// </summary>
    /// <param name="notes">The notes to convert.</param>
    /// <returns>A list of NoteDetailResponse objects.</returns>
    public static List<NoteDetailResponse> ToDetailResponses(this IEnumerable<Note> notes)
    {
        return notes.Select(n => n.ToResponse()).ToList();
    }
}
