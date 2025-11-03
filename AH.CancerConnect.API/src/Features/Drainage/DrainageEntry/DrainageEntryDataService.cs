using AH.CancerConnect.API.Features.Drainage.DrainageSetup;
using Microsoft.EntityFrameworkCore;

namespace AH.CancerConnect.API.Features.Drainage.DrainageEntry;

/// <summary>
/// Database implementation of drainage entry data service.
/// </summary>
public class DrainageEntryDataService : IDrainageEntryDataService
{
    private readonly CancerConnectDbContext _dbContext;
    private readonly ILogger<DrainageEntryDataService> _logger;

    public DrainageEntryDataService(CancerConnectDbContext dbContext, ILogger<DrainageEntryDataService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<int> CreateDrainageEntryAsync(DrainageEntryRequest request)
    {
        _logger.LogDebug("Creating drainage entry for drain {DrainId}", request.DrainId);

        // Validate request
        await ValidateDrainageEntryRequest(request);

        // Create the drainage entry using extension method
        var drainageEntry = request.ToEntity();

        // Save to database
        _dbContext.DrainageEntries.Add(drainageEntry);
        await _dbContext.SaveChangesAsync();

        _logger.LogDebug(
            "Successfully created drainage entry {EntryId} for drain {DrainId}",
            drainageEntry.Id,
            request.DrainId);

        return drainageEntry.Id;
    }

    /// <summary>
    /// Validates drainage entry request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    private async Task ValidateDrainageEntryRequest(DrainageEntryRequest request)
    {
        // Validate that the drain exists and is not archived
        var drain = await _dbContext.Drains
            .FirstOrDefaultAsync(d => d.Id == request.DrainId);

        if (drain == null)
        {
            throw new ArgumentException($"Drain with ID {request.DrainId} not found");
        }

        if (drain.IsArchived)
        {
            throw new InvalidOperationException($"Cannot create drainage entry for archived drain {request.DrainId}");
        }

        // Validate that at least one drain amount is provided
        if (!request.Drain1Amount.HasValue && !request.Drain2Amount.HasValue)
        {
            throw new ArgumentException("At least one drain amount (Drain 1 or Drain 2) must be provided");
        }

        // Validate empty date is not in the future
        if (request.EmptyDate > DateTime.UtcNow.AddDays(1)) // Allow some tolerance for timezone differences
        {
            throw new ArgumentException("Empty date cannot be in the future");
        }
    }
}