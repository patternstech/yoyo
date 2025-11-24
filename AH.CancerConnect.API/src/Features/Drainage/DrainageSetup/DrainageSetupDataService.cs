using Microsoft.EntityFrameworkCore;

namespace AH.CancerConnect.API.Features.Drainage.DrainageSetup;

/// <summary>
/// Database implementation of drainage setup data service.
/// </summary>
public class DrainageSetupDataService : IDrainageSetupDataService
{
    private readonly CancerConnectDbContext _dbContext;
    private readonly ILogger<DrainageSetupDataService> _logger;

    public DrainageSetupDataService(CancerConnectDbContext dbContext, ILogger<DrainageSetupDataService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<int> CreateDrainageSetupAsync(DrainageSetupRequest request)
    {
        _logger.LogDebug("Creating drainage setup for patient {PatientId}", request.PatientId);

        // Validate request
        ValidateDrainageSetupRequest(request);

        // Check if patient already has a drainage setup
        var existingSetup = await _dbContext.DrainageSetups
            .FirstOrDefaultAsync(ds => ds.PatientId == request.PatientId);

        if (existingSetup != null)
        {
            throw new InvalidOperationException($"Patient {request.PatientId} already has a drainage setup. Use update instead.");
        }

        // Create the drainage setup using extension method
        var drainageSetup = request.ToEntity();

        // Save to database
        _dbContext.DrainageSetups.Add(drainageSetup);
        await _dbContext.SaveChangesAsync();

        _logger.LogDebug(
            "Successfully created drainage setup {SetupId} for patient {PatientId}",
            drainageSetup.Id,
            request.PatientId);

        return drainageSetup.Id;
    }

    /// <inheritdoc />
    public async Task<DrainageSetupDetailResponse?> GetDrainageSetupByPatientAsync(int patientId)
    {
        _logger.LogDebug("Getting drainage setup for patient {PatientId}", patientId);

        var drainageSetup = await _dbContext.DrainageSetups
            .Include(ds => ds.Drains.Where(d => !d.IsArchived))
            .FirstOrDefaultAsync(ds => ds.PatientId == patientId);

        if (drainageSetup == null)
        {
            _logger.LogDebug("No drainage setup found for patient {PatientId}", patientId);
            return null;
        }

        return drainageSetup.ToDetailResponse();
    }

    /// <inheritdoc />
    public async Task<int> UpdateDrainageSetupAsync(DrainageSetupUpdateRequest request)
    {
        _logger.LogDebug("Updating drainage setup {SetupId} for patient {PatientId}", request.Id, request.PatientId);

        // Validate request
        ValidateDrainageSetupUpdateRequest(request);

        // Retrieve the existing drainage setup with drains
        var drainageSetup = await _dbContext.DrainageSetups
            .Include(ds => ds.Drains)
            .FirstOrDefaultAsync(ds => ds.Id == request.Id && ds.PatientId == request.PatientId);

        if (drainageSetup == null)
        {
            throw new KeyNotFoundException($"Drainage setup with ID {request.Id} not found for patient {request.PatientId}");
        }

        // Validate maximum active drains
        var activeRequests = request.Drains.Where(d => !d.IsArchived).ToList();
        if (activeRequests.Count > 4)
        {
            throw new ArgumentException("Cannot have more than 4 active drains");
        }

        // Update the drainage setup using extension method
        drainageSetup.UpdateFrom(request);

        await _dbContext.SaveChangesAsync();

        _logger.LogDebug(
            "Successfully updated drainage setup {SetupId} for patient {PatientId}",
            request.Id,
            request.PatientId);

        return drainageSetup.Id;
    }

    /// <inheritdoc />
    public async Task<bool> ArchiveDrainAsync(ArchiveDrainRequest request)
    {
        _logger.LogDebug("Archiving drain {DrainId} for patient {PatientId}", request.DrainId, request.PatientId);

        // Find the drain and validate ownership
        var drain = await _dbContext.Drains
            .Include(d => d.DrainageSetup)
            .FirstOrDefaultAsync(d => d.Id == request.DrainId && d.DrainageSetup.PatientId == request.PatientId);

        if (drain == null)
        {
            throw new KeyNotFoundException($"Drain with ID {request.DrainId} not found for patient {request.PatientId}");
        }

        if (drain.IsArchived)
        {
            _logger.LogDebug("Drain {DrainId} is already archived", request.DrainId);
            return true; // Already archived
        }

        // Archive the drain
        drain.IsArchived = true;
        drain.DateArchived = DateTime.Now;

        _logger.LogDebug("Successfully archived drain {DrainId}", request.DrainId);

        return true;
    }

    /// <summary>
    /// Validates drainage setup request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    private static void ValidateDrainageSetupRequest(DrainageSetupRequest request)
    {
        if (request.HasProviderGoalAmount && !request.GoalDrainageAmount.HasValue)
        {
            throw new ArgumentException("Goal drainage amount is required when provider has set a goal amount");
        }

        if (!request.HasProviderGoalAmount && request.GoalDrainageAmount.HasValue)
        {
            throw new ArgumentException("Goal drainage amount should not be provided when provider has not set a goal amount");
        }

        // Validate goal amount range if provided
        if (request.HasProviderGoalAmount && request.GoalDrainageAmount.HasValue)
        {
            if (request.GoalDrainageAmount.Value < 20 || request.GoalDrainageAmount.Value > 50)
            {
                throw new ArgumentException("Goal drainage amount must be between 20 and 50 mL");
            }
        }

        if (request.Drains.Count > 4)
        {
            throw new ArgumentException("Cannot add more than 4 drains");
        }

        if (request.Drains.Any(d => string.IsNullOrWhiteSpace(d.Name)))
        {
            throw new ArgumentException("All drains must have a name");
        }

        var duplicateNames = request.Drains
            .GroupBy(d => d.Name.Trim(), StringComparer.OrdinalIgnoreCase)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        if (duplicateNames.Any())
        {
            throw new ArgumentException($"Duplicate drain names are not allowed: {string.Join(", ", duplicateNames)}");
        }
    }

    /// <summary>
    /// Validates drainage setup update request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    private static void ValidateDrainageSetupUpdateRequest(DrainageSetupUpdateRequest request)
    {
        if (request.HasProviderGoalAmount && !request.GoalDrainageAmount.HasValue)
        {
            throw new ArgumentException("Goal drainage amount is required when provider has set a goal amount");
        }

        if (!request.HasProviderGoalAmount && request.GoalDrainageAmount.HasValue)
        {
            throw new ArgumentException("Goal drainage amount should not be provided when provider has not set a goal amount");
        }

        // Validate goal amount range if provided
        if (request.HasProviderGoalAmount && request.GoalDrainageAmount.HasValue)
        {
            if (request.GoalDrainageAmount.Value < 20 || request.GoalDrainageAmount.Value > 50)
            {
                throw new ArgumentException("Goal drainage amount must be between 20 and 50 mL");
            }
        }

        if (request.Drains.Count > 4)
        {
            throw new ArgumentException("Cannot have more than 4 drains");
        }

        if (request.Drains.Any(d => string.IsNullOrWhiteSpace(d.Name)))
        {
            throw new ArgumentException("All drains must have a name");
        }

        var duplicateNames = request.Drains
            .GroupBy(d => d.Name.Trim(), StringComparer.OrdinalIgnoreCase)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        if (duplicateNames.Any())
        {
            throw new ArgumentException($"Duplicate drain names are not allowed: {string.Join(", ", duplicateNames)}");
        }
    }
}