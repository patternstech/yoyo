using Microsoft.EntityFrameworkCore;

namespace AH.CancerConnect.AdminAPI.Features.ProviderPool;

/// <summary>
/// Database implementation of provider pool data service.
/// </summary>
public class ProviderPoolDataService : IProviderPoolDataService
{
    private readonly CancerConnectDbContext _dbContext;
    private readonly ILogger<ProviderPoolDataService> _logger;

    public ProviderPoolDataService(CancerConnectDbContext dbContext, ILogger<ProviderPoolDataService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProviderPoolListResponse>> GetProviderPoolsAsync()
    {
        _logger.LogDebug("Getting provider pools");

        var providerPools = await _dbContext.ProviderPools
            .Include(pp => pp.Providers)
            .OrderBy(pp => pp.Name)
            .ToListAsync();

        var response = providerPools.Select(pp => pp.ToListResponse()).ToList();

        _logger.LogDebug("Retrieved {Count} provider pools", providerPools.Count);

        return response;
    }

    /// <inheritdoc />
    public async Task<int> CreateProviderPoolAsync(ProviderPoolRequest request, string createdBy)
    {
        _logger.LogDebug("Creating provider pool {Name} by {CreatedBy}", request.Name, createdBy);

        // Validate request
        await ValidateProviderPoolRequestAsync(request);

        // Generate a unique ProviderPoolId
        var maxProviderPoolId = await _dbContext.ProviderPools
            .Select(pp => (int?)pp.ProviderPoolId)
            .MaxAsync() ?? 0;

        // Create the provider pool using extension method
        var providerPool = request.ToEntity(createdBy);
        providerPool.ProviderPoolId = maxProviderPoolId + 1;

        // Save to database
        _dbContext.ProviderPools.Add(providerPool);
        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully created provider pool with database ID {Id} and ProviderPoolId {ProviderPoolId}",
            providerPool.Id, providerPool.ProviderPoolId);

        return providerPool.Id;
    }

    /// <inheritdoc />
    public async Task<int> UpdateProviderPoolAsync(ProviderPoolUpdateRequest request)
    {
        _logger.LogDebug("Updating provider pool {Id}", request.Id);

        // Validate request
        await ValidateProviderPoolUpdateRequestAsync(request);

        // Retrieve the existing provider pool
        var providerPool = await _dbContext.ProviderPools
            .FirstOrDefaultAsync(pp => pp.Id == request.Id);

        if (providerPool == null)
        {
            throw new ArgumentException($"Provider pool with ID {request.Id} not found");
        }

        // Update the provider pool using extension method
        providerPool.UpdateFrom(request);

        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully updated provider pool {Id}", request.Id);

        return providerPool.Id;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteProviderPoolAsync(int id)
    {
        _logger.LogDebug("Deleting provider pool {Id}", id);

        var providerPool = await _dbContext.ProviderPools
            .Include(pp => pp.Providers)
            .FirstOrDefaultAsync(pp => pp.Id == id);

        if (providerPool == null)
        {
            _logger.LogWarning("Provider pool {Id} not found for deletion", id);
            return false;
        }

        // Check if there are any providers associated with this pool
        if (providerPool.Providers.Any())
        {
            throw new InvalidOperationException($"Cannot delete provider pool {id} because it has {providerPool.Providers.Count} associated providers");
        }

        // Hard delete since we removed IsActive
        _dbContext.ProviderPools.Remove(providerPool);
        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully deleted provider pool {Id}", id);

        return true;
    }

    /// <summary>
    /// Validates provider pool request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    private async Task ValidateProviderPoolRequestAsync(ProviderPoolRequest request)
    {
        // Check for duplicate name
        var nameExists = await _dbContext.ProviderPools
            .AnyAsync(pp => pp.Name == request.Name);

        if (nameExists)
        {
            throw new ArgumentException($"A provider pool with name '{request.Name}' already exists");
        }
    }

    /// <summary>
    /// Validates provider pool update request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    private async Task ValidateProviderPoolUpdateRequestAsync(ProviderPoolUpdateRequest request)
    {
        // Check for duplicate name (excluding current provider pool)
        var nameExists = await _dbContext.ProviderPools
            .AnyAsync(pp => pp.Name == request.Name && pp.Id != request.Id);

        if (nameExists)
        {
            throw new ArgumentException($"A provider pool with name '{request.Name}' already exists");
        }
    }
}
