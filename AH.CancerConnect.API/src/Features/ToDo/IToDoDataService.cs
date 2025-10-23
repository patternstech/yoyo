namespace AH.CancerConnect.API.Features.ToDo;

/// <summary>
/// Interface for ToDo data service operations.
/// </summary>
public interface IToDoDataService
{
    /// <summary>
    /// Create a new ToDo.
    /// </summary>
    /// <param name="request">ToDo request.</param>
    /// <returns>Created ToDo ID.</returns>
    Task<int> CreateToDoAsync(ToDoRequest request);

    /// <summary>
    /// Toggle completion status of a ToDo.
    /// </summary>
    /// <param name="todoId">ToDo ID.</param>
    /// <param name="patientId">Patient ID for validation.</param>
    /// <returns>Success status.</returns>
    Task<bool> ToggleCompletionAsync(int todoId, int patientId);

    /// <summary>
    /// Delete a ToDo.
    /// </summary>
    /// <param name="todoId">ToDo ID.</param>
    /// <param name="patientId">Patient ID for validation.</param>
    /// <returns>Success status.</returns>
    Task<bool> DeleteToDoAsync(int todoId, int patientId);

    /// <summary>
    /// Get all ToDos for a patient.
    /// </summary>
    /// <param name="patientId">Patient ID.</param>
    /// <returns>List of ToDos.</returns>
    Task<IEnumerable<ToDoDetailResponse>> GetToDosByPatientAsync(int patientId);
}
