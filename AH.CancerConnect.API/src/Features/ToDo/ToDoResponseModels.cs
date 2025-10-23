namespace AH.CancerConnect.API.Features.ToDo;

/// <summary>
/// Response model for ToDo operations.
/// </summary>
public class ToDoResponse
{
    public bool Success { get; set; }

    public int Id { get; set; }

    public List<string>? ValidationErrors { get; set; }
}

/// <summary>
/// Detailed ToDo response model.
/// </summary>
public class ToDoDetailResponse
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public DateTime DateCreated { get; set; }

    public string? Title { get; set; }

    public string Detail { get; set; } = string.Empty;

    public DateTime? Date { get; set; }

    public TimeSpan? Time { get; set; }

    public AlertType? Alert { get; set; }

    public bool IsCompleted { get; set; }
}
