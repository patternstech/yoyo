using Microsoft.EntityFrameworkCore;

namespace AH.CancerConnect.AdminAPI.Features.Provider;

/// <summary>
/// Database implementation of provider data service.
/// </summary>
public class ProviderDataService : IProviderDataService
{
    private readonly CancerConnectDbContext _dbContext;
    private readonly ILogger<ProviderDataService> _logger;

    public ProviderDataService(CancerConnectDbContext dbContext, ILogger<ProviderDataService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProviderDetailResponse>> GetProvidersAsync()
    {
        _logger.LogDebug("Getting providers");

        // Start with base query including provider pool
        var query = _dbContext.Providers.Include(p => p.ProviderPool).AsQueryable();

        // Simple ordering by last name, then first name
        var providers = await query
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .ToListAsync();

        var providerResponses = providers.Select(p => p.ToDetailResponse()).ToList();

        _logger.LogDebug("Retrieved {Count} providers", providers.Count);

        return providerResponses;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProviderPoolListResponse>> GetProviderPoolsAsync()
    {
        _logger.LogDebug("Getting provider pools");

        var query = _dbContext.ProviderPools.Include(pp => pp.Providers).AsQueryable();

        var providerPools = await query
            .OrderBy(pp => pp.Name)
            .ToListAsync();

        return providerPools.Select(pp => pp.ToListResponse());
    }

    /// <inheritdoc />
    public async Task<int> CreateProviderAsync(ProviderRequest request)
    {
        _logger.LogDebug("Creating provider {FirstName} {LastName} with ID {ProviderId}",
            request.FirstName, request.LastName, request.ProviderId);

        // Validate request
        await ValidateProviderRequestAsync(request);

        // Create the provider using extension method
        var provider = request.ToEntity();

        // Save to database
        _dbContext.Providers.Add(provider);
        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully created provider with database ID {Id}",
            provider.Id);

        return provider.Id;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteProviderAsync(int id)
    {
        _logger.LogDebug("Deleting provider {Id}", id);

        var provider = await _dbContext.Providers
            .FirstOrDefaultAsync(p => p.Id == id);

        if (provider == null)
        {
            _logger.LogWarning("Provider {Id} not found for deletion", id);
            return false;
        }

        // Soft delete - just mark as inactive
        provider.IsActive = false;
        provider.DateModified = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully deleted (marked inactive) provider {Id}", id);

        return true;
    }

    /// <inheritdoc />
    public async Task<int> UpdateProviderAsync(ProviderUpdateRequest request)
    {
        _logger.LogDebug("Updating provider {Id} with provider ID {ProviderId}",
            request.Id, request.ProviderId);

        // Validate request
        await ValidateProviderUpdateRequestAsync(request);

        // Retrieve the existing provider
        var provider = await _dbContext.Providers
            .FirstOrDefaultAsync(p => p.Id == request.Id);

        if (provider == null)
        {
            throw new ArgumentException($"Provider with ID {request.Id} not found");
        }

        // Update the provider using extension method
        provider.UpdateFrom(request);

        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully updated provider {Id}", request.Id);

        return provider.Id;
    }

    /// <summary>
    /// ProviderIdExistsAsync.
    /// </summary>
    /// <param name="providerId">The providerId to validate.</param>
    private async Task<bool> ProviderIdExistsAsync(string providerId, int? excludeId = null)
    {
        var query = _dbContext.Providers.Where(p => p.ProviderId == providerId);

        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    /// <summary>
    /// Validates provider request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    private async Task ValidateProviderRequestAsync(ProviderRequest request)
    {
        // Check for duplicate provider ID
        if (await ProviderIdExistsAsync(request.ProviderId))
        {
            throw new ArgumentException($"A provider with ID '{request.ProviderId}' already exists");
        }

        // Validate provider pool if specified
        if (request.ProviderPoolId.HasValue)
        {
            var poolExists = await _dbContext.ProviderPools
                .AnyAsync(pp => pp.Id == request.ProviderPoolId.Value && pp.IsActive);

            if (!poolExists)
            {
                throw new ArgumentException($"Provider pool with ID {request.ProviderPoolId} does not exist or is inactive");
            }
        }
    }

    /// <summary>
    /// Validates provider update request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    private async Task ValidateProviderUpdateRequestAsync(ProviderUpdateRequest request)
    {
        // Check for duplicate provider ID (excluding current provider)
        if (await ProviderIdExistsAsync(request.ProviderId, request.Id))
        {
            throw new ArgumentException($"A provider with ID '{request.ProviderId}' already exists");
        }

        // Validate provider pool if specified
        if (request.ProviderPoolId.HasValue)
        {
            var poolExists = await _dbContext.ProviderPools
                .AnyAsync(pp => pp.Id == request.ProviderPoolId.Value && pp.IsActive);

            if (!poolExists)
            {
                throw new ArgumentException($"Provider pool with ID {request.ProviderPoolId} does not exist or is inactive");
            }
        }
    }

}