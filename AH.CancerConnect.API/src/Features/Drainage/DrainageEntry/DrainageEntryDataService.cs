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
    public async Task<List<int>> CreateDrainageEntryAsync(DrainageEntryRequest request)
    {
        _logger.LogDebug("Creating drainage entries for patient {PatientId} with {DrainCount} drains", 
            request.PatientId, request.DrainEntries.Count);

        // Validate request
        await ValidateDrainageEntryRequest(request);

        var createdEntryIds = new List<int>();

        // Create drainage entry for each drain
        foreach (var drainEntry in request.DrainEntries)
        {
            var drainageEntry = new DrainageEntry
            {
                DrainId = drainEntry.DrainId,
                EmptyDate = request.EmptyDate,
                Amount = drainEntry.Amount,
                Note = request.Note,
                IsArchived = false,
                DateCreated = DateTime.Now,
            };

            _dbContext.DrainageEntries.Add(drainageEntry);
            await _dbContext.SaveChangesAsync();
            
            createdEntryIds.Add(drainageEntry.Id);
            
            _logger.LogDebug(
                "Successfully created drainage entry {EntryId} for drain {DrainId}",
                drainageEntry.Id,
                drainEntry.DrainId);
        }

        return createdEntryIds;
    }

    /// <summary>
    /// Validates drainage entry request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    private async Task ValidateDrainageEntryRequest(DrainageEntryRequest request)
    {
        // Validate that the patient exists
        var patientExists = await _dbContext.Patients
            .AnyAsync(p => p.Id == request.PatientId);

        if (!patientExists)
        {
            throw new KeyNotFoundException($"Patient with ID {request.PatientId} not found");
        }

        // Validate each drain in the request
        foreach (var drainEntry in request.DrainEntries)
        {
            // Validate that the drain exists and is not archived
            var drain = await _dbContext.Drains
                .Include(d => d.DrainageSetup)
                .FirstOrDefaultAsync(d => d.Id == drainEntry.DrainId);

            if (drain == null)
            {
                throw new KeyNotFoundException($"Drain with ID {drainEntry.DrainId} not found");
            }

            if (drain.IsArchived)
            {
                throw new InvalidOperationException($"Cannot create drainage entry for archived drain {drainEntry.DrainId}");
            }

            // Validate drain belongs to the patient
            if (drain.DrainageSetup.PatientId != request.PatientId)
            {
                throw new InvalidOperationException($"Drain {drainEntry.DrainId} does not belong to patient {request.PatientId}");
            }

            // Validate amount is between 0 and 100 mL
            if (drainEntry.Amount < 0 || drainEntry.Amount > 100)
            {
                throw new ArgumentException($"Drainage entry amount for drain {drainEntry.DrainId} must be between 0 and 100 mL");
            }
        }

        // Validate empty date is not in the future
        if (request.EmptyDate > DateTime.Now.AddDays(1))
        {
            throw new ArgumentException("Empty date cannot be in the future");
        }
    }

    /// <inheritdoc />
    public async Task<bool> UpdateDrainageEntryAsync(DrainageEntryUpdateRequest request)
    {
        _logger.LogDebug("Updating drainage entries for session");

        // Validate empty date is not in the future
        if (request.EmptyDate > DateTime.Now.AddDays(1))
        {
            throw new ArgumentException("Empty date cannot be in the future");
        }

        // Retrieve all entry IDs to validate they exist
        var entryIds = request.DrainEntries.Select(e => e.EntryId).ToList();
        var entries = await _dbContext.DrainageEntries
            .Include(e => e.Drain)
            .Where(e => entryIds.Contains(e.Id))
            .ToListAsync();

        // Validate all entries exist
        if (entries.Count != entryIds.Count)
        {
            var foundIds = entries.Select(e => e.Id).ToList();
            var missingIds = entryIds.Except(foundIds);
            throw new KeyNotFoundException($"Drainage entries not found: {string.Join(", ", missingIds)}");
        }

        // Update each entry
        foreach (var updateItem in request.DrainEntries)
        {
            var entry = entries.First(e => e.Id == updateItem.EntryId);

            // Validate that the entry is not archived
            if (entry.IsArchived)
            {
                throw new InvalidOperationException($"Cannot update archived drainage entry {entry.Id}");
            }

            // Validate that the drain is not archived
            if (entry.Drain.IsArchived)
            {
                throw new InvalidOperationException($"Cannot update drainage entry for archived drain {entry.DrainId}");
            }

            // Validate amount is between 0 and 100 mL
            if (updateItem.Amount < 0 || updateItem.Amount > 100)
            {
                throw new ArgumentException("Drainage entry amount must be between 0 and 100 mL");
            }

            // Update the entry
            entry.EmptyDate = request.EmptyDate;
            entry.Amount = updateItem.Amount;
            entry.Note = request.Note;
        }

        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully updated {Count} drainage entries", request.DrainEntries.Count);

        return true;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<DrainageSessionResponse>> GetDrainageSessionsByPatientAsync(int patientId)
    {
        _logger.LogDebug("Retrieving grouped drainage sessions for patient {PatientId}", patientId);

        // Get all drains for the patient through drainage setup
        var drains = await _dbContext.Drains
            .Include(d => d.DrainageSetup)
            .Where(d => d.DrainageSetup.PatientId == patientId)
            .ToListAsync();

        var drainIds = drains.Select(d => d.Id).ToList();

        // Get all entries for those drains (non-archived only)
        var entries = await _dbContext.DrainageEntries
            .Include(e => e.Drain)
            .Where(e => drainIds.Contains(e.DrainId) && !e.IsArchived)
            .OrderByDescending(e => e.EmptyDate)
            .ThenBy(e => e.DateCreated)
            .ToListAsync();

        // Group entries by EmptyDate and Note to create sessions
        var sessions = entries
            .GroupBy(e => new { e.EmptyDate, e.Note, e.DateCreated })
            .Select(g => new DrainageSessionResponse
            {
                DrainageEntryId = g.First().Id,
                PatientId = patientId,
                EmptyDate = g.Key.EmptyDate,
                Note = g.Key.Note,
                DrainEntries = g.Select(e => new DrainEntryDetail
                {
                    DrainId = e.DrainId,
                    Amount = e.Amount,
                    DrainName = e.Drain.Name,
                    IsArchived = e.Drain.IsArchived,
                }).ToList(),
            })
            .OrderByDescending(s => s.EmptyDate)
            .ToList();

        _logger.LogDebug("Retrieved {Count} drainage sessions for patient {PatientId}", sessions.Count, patientId);

        return sessions;
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
        entry.DateArchived = DateTime.Now;

        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully archived drainage entry {EntryId}", entryId);

        return true;
    }
}