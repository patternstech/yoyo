namespace AH.CancerConnect.API.Features.Drainage.DrainageSetup;

/// <summary>
/// Extension methods for mapping drainage setup requests to entities.
/// </summary>
public static class DrainageSetupExtensions
{
    /// <summary>
    /// Converts a DrainageSetupRequest to a DrainageSetup entity.
    /// </summary>
    /// <param name="request">The drainage setup request.</param>
    /// <returns>A new DrainageSetup entity.</returns>
    public static DrainageSetup ToEntity(this DrainageSetupRequest request)
    {
        var setup = new DrainageSetup
        {
            PatientId = request.PatientId,
            HasProviderGoalAmount = request.HasProviderGoalAmount,
            GoalDrainageAmount = request.HasProviderGoalAmount ? request.GoalDrainageAmount : 30, // Default 30 mL if no provider goal
            ProviderInstructions = request.ProviderInstructions,
            DateCreated = DateTime.Now,
            DateModified = DateTime.Now,
        };

        // Add drains
        setup.Drains = request.Drains.Select(d => d.ToEntity()).ToList();

        return setup;
    }

    /// <summary>
    /// Converts a DrainRequest to a Drain entity.
    /// </summary>
    /// <param name="request">The drain request.</param>
    /// <returns>A new Drain entity.</returns>
    public static Drain ToEntity(this DrainRequest request)
    {
        return new Drain
        {
            Name = request.Name,
            IsArchived = request.IsArchived,
            DateCreated = DateTime.Now,
            DateArchived = request.IsArchived ? DateTime.Now : null,
        };
    }

    /// <summary>
    /// Converts a DrainageSetup entity to DrainageSetupDetailResponse.
    /// </summary>
    /// <param name="setup">The drainage setup entity.</param>
    /// <returns>A drainage setup detail response.</returns>
    public static DrainageSetupDetailResponse ToDetailResponse(this DrainageSetup setup)
    {
        return new DrainageSetupDetailResponse
        {
            Id = setup.Id,
            PatientId = setup.PatientId,
            HasProviderGoalAmount = setup.HasProviderGoalAmount,
            GoalDrainageAmount = setup.GoalDrainageAmount,
            ProviderInstructions = setup.ProviderInstructions,
            DateCreated = setup.DateCreated,
            DateModified = setup.DateModified,
            Drains = setup.Drains.Select(d => d.ToResponse()).ToList(),
        };
    }

    /// <summary>
    /// Converts a Drain entity to DrainResponse.
    /// </summary>
    /// <param name="drain">The drain entity.</param>
    /// <returns>A drain response.</returns>
    public static DrainResponse ToResponse(this Drain drain)
    {
        return new DrainResponse
        {
            Id = drain.Id,
            Name = drain.Name,
            IsArchived = drain.IsArchived,
            DateCreated = drain.DateCreated,
            DateArchived = drain.DateArchived,
        };
    }

    /// <summary>
    /// Updates an existing DrainageSetup entity from a DrainageSetupUpdateRequest.
    /// </summary>
    /// <param name="setup">The existing drainage setup entity.</param>
    /// <param name="request">The update request.</param>
    public static void UpdateFrom(this DrainageSetup setup, DrainageSetupUpdateRequest request)
    {
        setup.HasProviderGoalAmount = request.HasProviderGoalAmount;
        setup.GoalDrainageAmount = request.HasProviderGoalAmount ? request.GoalDrainageAmount : 30; // Default 30 mL if no provider goal
        setup.ProviderInstructions = request.ProviderInstructions;
        setup.DateModified = DateTime.Now;

        // Update drains - this is complex as we need to handle additions, updates, and deletions
        UpdateDrains(setup, request.Drains);
    }

    /// <summary>
    /// Updates the drains collection for a drainage setup.
    /// </summary>
    /// <param name="setup">The drainage setup entity.</param>
    /// <param name="drainRequests">The updated drain requests.</param>
    private static void UpdateDrains(DrainageSetup setup, List<DrainRequest> drainRequests)
    {
        var existingDrains = setup.Drains.ToList();

        // Update existing drains
        foreach (var request in drainRequests.Where(d => d.Id.HasValue))
        {
            var existingDrain = existingDrains.FirstOrDefault(d => d.Id == request.Id!.Value);
            if (existingDrain == null)
            {
                throw new KeyNotFoundException($"Drain with ID {request.Id!.Value} not found in this drainage setup");
            }

            existingDrain.Name = request.Name;
            if (request.IsArchived && !existingDrain.IsArchived)
            {
                existingDrain.IsArchived = true;
                existingDrain.DateArchived = DateTime.Now;
            }
            else if (!request.IsArchived && existingDrain.IsArchived)
            {
                existingDrain.IsArchived = false;
                existingDrain.DateArchived = null;
            }
        }

        // Add new drains (those without an ID)
        var newDrains = drainRequests.Where(d => !d.Id.HasValue).ToList();
        
        if (newDrains.Any())
        {
            // Add the new drains
            foreach (var request in newDrains)
            {
                var newDrain = request.ToEntity();
                newDrain.DrainageSetupId = setup.Id;
                setup.Drains.Add(newDrain);
            }
        }
    }
}