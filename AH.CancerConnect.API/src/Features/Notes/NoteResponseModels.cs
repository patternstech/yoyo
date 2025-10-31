namespace AH.CancerConnect.API.Features.Notes;

/// <summary>
/// Response model for note operations.
/// </summary>
public class NoteResponse
{
    public bool Success { get; set; }

    public int Id { get; set; }

    public List<string>? ValidationErrors { get; set; }
}

/// <summary>
/// Response model for note details.
/// </summary>
public class NoteDetailResponse
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public string? Title { get; set; }

    public string NoteText { get; set; } = string.Empty;
}
