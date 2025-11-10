namespace AH.CancerConnect.API.Features.ToDo;

/// <summary>
/// Extension methods for mapping between ToDo entities and DTOs.
/// </summary>
public static class ToDoExtensions
{
    /// <summary>
    /// Converts ToDoRequest to ToDo entity.
    /// </summary>
    /// <param name="request">The todo request.</param>
    /// <returns>A new ToDo entity.</returns>
    public static ToDo ToEntity(this ToDoRequest request)
    {
        return new ToDo
        {
            PatientId = request.PatientId,
            Title = request.Title,
            Detail = request.Detail,
            Date = request.Date,
            Time = request.Time,
            Alert = request.Alert,
            IsCompleted = false,
            DateCreated = DateTime.Now,
        };
    }

    /// <summary>
    /// Converts ToDo entity to ToDoDetailResponse.
    /// </summary>
    /// <param name="todo">The todo entity.</param>
    /// <returns>A ToDoDetailResponse object.</returns>
    public static ToDoDetailResponse ToResponse(this ToDo todo)
    {
        return new ToDoDetailResponse
        {
            Id = todo.Id,
            PatientId = todo.PatientId,
            DateCreated = todo.DateCreated,
            Title = todo.Title,
            Detail = todo.Detail,
            Date = todo.Date,
            Time = todo.Time,
            Alert = todo.Alert,
            IsCompleted = todo.IsCompleted,
        };
    }

    /// <summary>
    /// Converts list of ToDo entities to list of ToDoDetailResponse.
    /// </summary>
    /// <param name="todos">The todos to convert.</param>
    /// <returns>A list of ToDoDetailResponse objects.</returns>
    public static List<ToDoDetailResponse> ToDetailResponses(this IEnumerable<ToDo> todos)
    {
        return todos.Select(todo => todo.ToResponse()).ToList();
    }
}
