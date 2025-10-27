using Microsoft.EntityFrameworkCore;

namespace AH.CancerConnect.AdminAPI.Features.SymptomConfiguration;

/// <summary>
/// Database implementation of symptom configuration data service.
/// </summary>
public class SymptomConfigurationDataService : ISymptomConfigurationDataService
{
    private readonly CancerConnectDbContext _dbContext;
    private readonly ILogger<SymptomConfigurationDataService> _logger;

    public SymptomConfigurationDataService(CancerConnectDbContext dbContext, ILogger<SymptomConfigurationDataService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SymptomConfigurationResponse>> GetSymptomConfigurationsAsync()
    {
        _logger.LogDebug("Getting symptom configurations");

        var configurations = await _dbContext.SymptomConfigurations
            .Include(sc => sc.Symptom)
            .Where(sc => sc.IsActive)
            .OrderBy(sc => sc.Symptom!.Name)
            .ToListAsync();

        var responses = configurations.Select(c => c.ToResponse()).ToList();

        _logger.LogDebug("Retrieved {Count} symptom configurations", configurations.Count);

        return responses;
    }
}
