using Microsoft.EntityFrameworkCore;

namespace AH.CancerConnect.API.Features.Notes;

/// <summary>
/// Database implementation of note data service.
/// </summary>
public class NoteDataService : INoteDataService
{
    private readonly CancerConnectDbContext _dbContext;
    private readonly ILogger<NoteDataService> _logger;

    public NoteDataService(CancerConnectDbContext dbContext, ILogger<NoteDataService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<int> CreateNoteAsync(NoteRequest request)
    {
        _logger.LogDebug("Creating note for patient {PatientId}", request.PatientId);

        // Validate note text
        ValidateNoteText(request.NoteText);

        // Create the note using extension method
        var note = request.ToEntity();

        // Save to database
        _dbContext.Notes.Add(note);
        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully created note {NoteId} for patient {PatientId}", note.Id, request.PatientId);

        return note.Id;
    }

    /// <inheritdoc />
    public async Task<int> UpdateNoteAsync(NoteUpdateRequest request)
    {
        _logger.LogDebug("Updating note {NoteId} for patient {PatientId}", request.Id, request.PatientId);

        // Validate note text
        ValidateNoteText(request.NoteText);

        // Retrieve the existing note
        var note = await _dbContext.Notes
            .FirstOrDefaultAsync(n => n.Id == request.Id);

        if (note == null)
        {
            throw new ArgumentException($"Note with ID {request.Id} not found.");
        }

        // Verify the note belongs to the specified patient
        if (note.PatientId != request.PatientId)
        {
            throw new ArgumentException($"Note {request.Id} does not belong to patient {request.PatientId}.");
        }

        // Update note properties using extension method
        note.UpdateFrom(request);

        // Save tracked changes
        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully updated note {NoteId} for patient {PatientId}", note.Id, request.PatientId);

        return note.Id;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteNoteAsync(int noteId, int patientId)
    {
        _logger.LogDebug("Deleting note {NoteId} for patient {PatientId}", noteId, patientId);

        // Retrieve the existing note
        var note = await _dbContext.Notes
            .FirstOrDefaultAsync(n => n.Id == noteId);

        if (note == null)
        {
            throw new ArgumentException($"Note with ID {noteId} not found.");
        }

        // Verify the note belongs to the specified patient
        if (note.PatientId != patientId)
        {
            throw new ArgumentException($"Note {noteId} does not belong to patient {patientId}.");
        }

        // Remove the note
        _dbContext.Notes.Remove(note);
        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully deleted note {NoteId} for patient {PatientId}", noteId, patientId);

        return true;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<NoteDetailResponse>> GetNotesByPatientAsync(int patientId)
    {
        _logger.LogDebug("Retrieving notes for patient {PatientId}", patientId);

        var notes = await _dbContext.Notes
            .Where(n => n.PatientId == patientId)
            .ToListAsync();

        _logger.LogDebug("Retrieved {Count} notes for patient {PatientId}", notes.Count, patientId);

        return notes.ToDetailResponses();
    }

    /// <summary>
    /// Validates note text length.
    /// </summary>
    /// <param name="noteText">The note text to validate.</param>
    private void ValidateNoteText(string noteText)
    {
        if (string.IsNullOrWhiteSpace(noteText))
        {
            throw new ArgumentException("Note text cannot be empty.");
        }

        if (noteText.Length > 1000)
        {
            throw new ArgumentException($"Note text cannot exceed 1000 characters. Current length: {noteText.Length}");
        }
    }
}
