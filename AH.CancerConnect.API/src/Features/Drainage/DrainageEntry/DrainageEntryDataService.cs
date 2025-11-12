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
        if (request.EmptyDate > DateTime.Now.AddDays(1))
        {
            throw new ArgumentException("Empty date cannot be in the future");
        }

        // Validate amount is between 0 and 100 mL
        if (request.Amount < 0 || request.Amount > 100)
        {
            throw new ArgumentException("Drainage entry amount must be between 0 and 100 mL");
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
    public async Task<IEnumerable<DrainageEntryDetailResponse>> GetDrainageEntriesByPatientAsync(int patientId)
    {
        _logger.LogDebug("Retrieving all drainage entries for patient {PatientId}", patientId);

        // Get all drains for the patient through drainage setup
        var drainIds = await _dbContext.Drains
            .Where(d => d.DrainageSetup.PatientId == patientId)
            .Select(d => d.Id)
            .ToListAsync();

        // Get all entries for those drains
        var entries = await _dbContext.DrainageEntries
            .Where(e => drainIds.Contains(e.DrainId))
            .OrderByDescending(e => e.EmptyDate)
            .ToListAsync();

        _logger.LogDebug("Retrieved {Count} drainage entries for patient {PatientId}", entries.Count, patientId);

        return entries.Select(e => e.ToDetailResponse());
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