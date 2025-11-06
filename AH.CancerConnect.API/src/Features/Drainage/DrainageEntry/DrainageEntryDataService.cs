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
            throw new KeyNotFoundException($"Drain with ID {request.DrainId} not found");
        }

        if (drain.IsArchived)
        {
            throw new InvalidOperationException($"Cannot create drainage entry for archived drain {request.DrainId}");
        }

        // Validate empty date is not in the future
        if (request.EmptyDate > DateTime.UtcNow.AddDays(1))
        {
            throw new ArgumentException("Empty date cannot be in the future");
        }
    }

    /// <inheritdoc />
    public async Task<bool> UpdateDrainageEntryAsync(int entryId, DrainageEntryUpdateRequest request)
    {
        _logger.LogDebug("Updating drainage entry {EntryId}", entryId);

        // Retrieve the existing drainage entry
        var entry = await _dbContext.DrainageEntries
            .Include(e => e.Drain)
            .FirstOrDefaultAsync(e => e.Id == entryId);

        if (entry == null)
        {
            throw new KeyNotFoundException($"Drainage entry with ID {entryId} not found");
        }

        // Validate that the entry is not archived
        if (entry.IsArchived)
        {
            throw new InvalidOperationException($"Cannot update archived drainage entry {entryId}");
        }

        // Validate that the drain is not archived
        if (entry.Drain.IsArchived)
        {
            throw new InvalidOperationException($"Cannot update drainage entry for archived drain {entry.DrainId}");
        }

        // Validate empty date is not in the future
        if (request.EmptyDate > DateTime.UtcNow.AddDays(1))
        {
            throw new ArgumentException("Empty date cannot be in the future");
        }

        // Update the entry
        entry.EmptyDate = request.EmptyDate;
        entry.Amount = request.Amount;
        entry.Note = request.Note;

        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully updated drainage entry {EntryId}", entryId);

        return true;
    }

    /// <inheritdoc />
    public async Task<DrainageEntryDetailResponse> GetDrainageEntryByIdAsync(int entryId)
    {
        _logger.LogDebug("Retrieving drainage entry {EntryId}", entryId);

        var entry = await _dbContext.DrainageEntries
            .FirstOrDefaultAsync(e => e.Id == entryId);

        if (entry == null)
        {
            throw new KeyNotFoundException($"Drainage entry with ID {entryId} not found");
        }

        _logger.LogDebug("Successfully retrieved drainage entry {EntryId}", entryId);

        return entry.ToDetailResponse();
    }

    /// <inheritdoc />
    public async Task<bool> DeleteDrainageEntryAsync(int entryId)
    {
        _logger.LogDebug("Deleting drainage entry {EntryId}", entryId);

        var entry = await _dbContext.DrainageEntries
            .FirstOrDefaultAsync(e => e.Id == entryId);

        if (entry == null)
        {
            throw new KeyNotFoundException($"Drainage entry with ID {entryId} not found");
        }

        if (entry.IsArchived)
        {
            throw new InvalidOperationException($"Drainage entry {entryId} is already archived");
        }

        // Archive the entry instead of hard delete
        entry.IsArchived = true;
        entry.DateArchived = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully archived drainage entry {EntryId}", entryId);

        return true;
    }
}