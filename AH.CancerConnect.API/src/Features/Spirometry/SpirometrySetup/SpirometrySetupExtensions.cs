namespace AH.CancerConnect.API.Features.Spirometry.SpirometrySetup;

/// <summary>
/// Extension methods for mapping spirometry setup requests to entities.
/// </summary>
public static class SpirometrySetupExtensions
{
    /// <summary>
    /// Converts a SpirometrySetupRequest to a SpirometrySetup entity.
    /// </summary>
    /// <param name="request">The spirometry setup request.</param>
    /// <returns>A new SpirometrySetup entity.</returns>
    public static SpirometrySetup ToEntity(this SpirometrySetupRequest request)
    {
        return new SpirometrySetup
        {
            PatientId = request.PatientId,
            CapacityGoal = request.CapacityGoal,
            ProviderInstructions = request.ProviderInstructions,
        };
    }

    /// <summary>
    /// Converts a SpirometrySetup entity to SpirometrySetupDetailResponse.
    /// </summary>
    /// <param name="setup">The spirometry setup entity.</param>
    /// <returns>A SpirometrySetupDetailResponse object.</returns>
    public static SpirometrySetupDetailResponse ToDetailResponse(this SpirometrySetup setup)
    {
        return new SpirometrySetupDetailResponse
        {
            Id = setup.Id,
            PatientId = setup.PatientId,
            CapacityGoal = setup.CapacityGoal,
            ProviderInstructions = setup.ProviderInstructions,
        };
    }

    /// <summary>
    /// Updates an existing SpirometrySetup entity from a SpirometrySetupUpdateRequest.
    /// </summary>
    /// <param name="setup">The existing spirometry setup entity.</param>
    /// <param name="request">The update request.</param>
    public static void UpdateFrom(this SpirometrySetup setup, SpirometrySetupUpdateRequest request)
    {
        setup.CapacityGoal = request.CapacityGoal;
        setup.ProviderInstructions = request.ProviderInstructions;
    }
}
