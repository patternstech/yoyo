using Microsoft.EntityFrameworkCore;

namespace AH.CancerConnect.API.Features.Spirometry.SpirometrySetup;

/// <summary>
/// Database implementation of spirometry setup data service.
/// </summary>
public class SpirometrySetupDataService : ISpirometrySetupDataService
{
    private readonly CancerConnectDbContext _dbContext;
    private readonly ILogger<SpirometrySetupDataService> _logger;

    public SpirometrySetupDataService(CancerConnectDbContext dbContext, ILogger<SpirometrySetupDataService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<int> CreateSpirometrySetupAsync(SpirometrySetupRequest request)
    {
        _logger.LogDebug("Creating spirometry setup for patient {PatientId}", request.PatientId);

        // Check if patient already has a spirometry setup
        var existingSetup = await _dbContext.SpirometrySetups
            .FirstOrDefaultAsync(ss => ss.PatientId == request.PatientId);

        if (existingSetup != null)
        {
            throw new InvalidOperationException($"Patient {request.PatientId} already has a spirometry setup. Use update instead.");
        }

        // Create the spirometry setup using extension method
        var spirometrySetup = request.ToEntity();

        // Save to database
        _dbContext.SpirometrySetups.Add(spirometrySetup);
        await _dbContext.SaveChangesAsync();

        _logger.LogDebug(
            "Successfully created spirometry setup {SetupId} for patient {PatientId}",
            spirometrySetup.Id,
            request.PatientId);

        return spirometrySetup.Id;
    }

    /// <inheritdoc />
    public async Task<SpirometrySetupDetailResponse?> GetSpirometrySetupByPatientAsync(int patientId)
    {
        _logger.LogDebug("Getting spirometry setup for patient {PatientId}", patientId);

        var spirometrySetup = await _dbContext.SpirometrySetups
            .FirstOrDefaultAsync(ss => ss.PatientId == patientId);

        if (spirometrySetup == null)
        {
            _logger.LogDebug("No spirometry setup found for patient {PatientId}", patientId);
            return null;
        }

        return spirometrySetup.ToDetailResponse();
    }

    /// <inheritdoc />
    public async Task<int> UpdateSpirometrySetupAsync(SpirometrySetupUpdateRequest request)
    {
        _logger.LogDebug("Updating spirometry setup {SetupId} for patient {PatientId}", request.Id, request.PatientId);

        // Retrieve the existing spirometry setup
        var spirometrySetup = await _dbContext.SpirometrySetups
            .FirstOrDefaultAsync(ss => ss.Id == request.Id && ss.PatientId == request.PatientId);

        if (spirometrySetup == null)
        {
            throw new KeyNotFoundException($"Spirometry setup with ID {request.Id} not found for patient {request.PatientId}");
        }

        // Update the spirometry setup using extension method
        spirometrySetup.UpdateFrom(request);

        await _dbContext.SaveChangesAsync();

        _logger.LogDebug(
            "Successfully updated spirometry setup {SetupId} for patient {PatientId}",
            request.Id,
            request.PatientId);

        return spirometrySetup.Id;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteSpirometrySetupAsync(int patientId)
    {
        _logger.LogDebug("Deleting spirometry setup for patient {PatientId}", patientId);

        // Retrieve the existing spirometry setup
        var spirometrySetup = await _dbContext.SpirometrySetups
            .FirstOrDefaultAsync(ss => ss.PatientId == patientId);

        if (spirometrySetup == null)
        {
            throw new KeyNotFoundException($"No spirometry setup found for patient {patientId}");
        }

        // Remove the spirometry setup
        _dbContext.SpirometrySetups.Remove(spirometrySetup);
        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully deleted spirometry setup for patient {PatientId}", patientId);

        return true;
    }

}
