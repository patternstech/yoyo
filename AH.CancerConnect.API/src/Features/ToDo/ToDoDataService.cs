using Microsoft.EntityFrameworkCore;

namespace AH.CancerConnect.API.Features.ToDo;

/// <summary>
/// Database implementation of ToDo data service.
/// </summary>
public class ToDoDataService : IToDoDataService
{
    private readonly CancerConnectDbContext _dbContext;
    private readonly ILogger<ToDoDataService> _logger;

    public ToDoDataService(CancerConnectDbContext dbContext, ILogger<ToDoDataService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<int> CreateToDoAsync(ToDoRequest request)
    {
        _logger.LogDebug("Creating ToDo for patient {PatientId}", request.PatientId);

        // Validate detail
        ValidateDetail(request.Detail);

        // Create the ToDo using extension method
        var todo = request.ToEntity();

        // Save to database
        _dbContext.ToDos.Add(todo);
        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully created ToDo {ToDoId} for patient {PatientId}", todo.Id, request.PatientId);

        return todo.Id;
    }

    /// <inheritdoc />
    public async Task<bool> ToggleCompletionAsync(int todoId, int patientId)
    {
        _logger.LogDebug("Toggling completion for ToDo {ToDoId} for patient {PatientId}", todoId, patientId);

        // Retrieve the existing ToDo
        var todo = await _dbContext.ToDos
            .FirstOrDefaultAsync(t => t.Id == todoId);

        if (todo == null)
        {
            throw new ArgumentException($"ToDo with ID {todoId} not found.");
        }

        // Verify the ToDo belongs to the specified patient
        if (todo.PatientId != patientId)
        {
            throw new ArgumentException($"ToDo {todoId} does not belong to patient {patientId}.");
        }

        // Toggle completion status
        todo.IsCompleted = !todo.IsCompleted;
        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully toggled completion for ToDo {ToDoId} to {IsCompleted}", todoId, todo.IsCompleted);

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteToDoAsync(int todoId, int patientId)
    {
        _logger.LogDebug("Deleting ToDo {ToDoId} for patient {PatientId}", todoId, patientId);

        // Retrieve the existing ToDo
        var todo = await _dbContext.ToDos
            .FirstOrDefaultAsync(t => t.Id == todoId);

        if (todo == null)
        {
            throw new ArgumentException($"ToDo with ID {todoId} not found.");
        }

        // Verify the ToDo belongs to the specified patient
        if (todo.PatientId != patientId)
        {
            throw new ArgumentException($"ToDo {todoId} does not belong to patient {patientId}.");
        }

        // Remove the ToDo
        _dbContext.ToDos.Remove(todo);
        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully deleted ToDo {ToDoId} for patient {PatientId}", todoId, patientId);

        return true;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ToDoDetailResponse>> GetToDosByPatientAsync(int patientId)
    {
        _logger.LogDebug("Retrieving ToDos for patient {PatientId}", patientId);

        var todos = await _dbContext.ToDos
            .Where(t => t.PatientId == patientId)
            .OrderByDescending(t => t.DateCreated)
            .ToListAsync();

        _logger.LogDebug("Retrieved {Count} ToDos for patient {PatientId}", todos.Count, patientId);

        return todos.ToDetailResponses();
    }

    /// <summary>
    /// Validates ToDo detail text.
    /// </summary>
    /// <param name="detail">The detail text to validate.</param>
    private void ValidateDetail(string detail)
    {
        if (string.IsNullOrWhiteSpace(detail))
        {
            throw new ArgumentException("Detail cannot be empty.");
        }
    }
}
