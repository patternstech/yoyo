namespace AH.CancerConnect.AdminAPI.Features.SymptomConfiguration;

/// <summary>
/// Extension methods for symptom configuration entities.
/// </summary>
public static class SymptomConfigurationExtensions
{
    /// <summary>
    /// Converts a SymptomConfiguration entity to a response model.
    /// </summary>
    /// <param name="config">The symptom configuration entity.</param>
    /// <returns>The response model.</returns>
    public static SymptomConfigurationResponse ToResponse(this SymptomConfiguration config)
    {
        return new SymptomConfigurationResponse
        {
            Id = config.Id,
            SymptomId = config.SymptomId,
            SymptomName = config.Symptom?.Name ?? string.Empty,
            AlertTrigger = config.AlertTrigger,
            FollowUp = config.FollowUp,
            Question = config.Question,
            Created = config.Created,
            Status = config.IsActive ? "Active" : "Inactive",
        };
    }
}
