using AH.CancerConnect.API.Features.Spirometry.SpirometrySetup;
using Microsoft.EntityFrameworkCore;

namespace AH.CancerConnect.API.Features.Spirometry.SpirometryEntry;

/// <summary>
/// Database implementation of spirometry entry data service.
/// </summary>
public class SpirometryEntryDataService : ISpirometryEntryDataService
{
    private readonly CancerConnectDbContext _dbContext;
    private readonly ILogger<SpirometryEntryDataService> _logger;

    public SpirometryEntryDataService(CancerConnectDbContext dbContext, ILogger<SpirometryEntryDataService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<int> CreateSpirometryEntryAsync(SpirometryEntryRequest request)
    {
        _logger.LogDebug("Creating spirometry entry for patient {PatientId}", request.PatientId);

        // Validate request
        await ValidateSpirometryEntryRequest(request);

        // Create the spirometry entry using extension method
        var spirometryEntry = request.ToEntity();

        // Save to database
        _dbContext.SpirometryEntries.Add(spirometryEntry);
        await _dbContext.SaveChangesAsync();

        _logger.LogDebug(
            "Successfully created spirometry entry {EntryId} for patient {PatientId}",
            spirometryEntry.Id,
            request.PatientId);

        return spirometryEntry.Id;
    }

    /// <summary>
    /// Validates spirometry entry request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    private async Task ValidateSpirometryEntryRequest(SpirometryEntryRequest request)
    {
        // Validate that the patient exists
        var patientExists = await _dbContext.Patients
            .AnyAsync(p => p.Id == request.PatientId);

        if (!patientExists)
        {
            throw new KeyNotFoundException($"Patient with ID {request.PatientId} not found");
        }

        // Validate test date is not in the future
        if (request.TestDate > DateOnly.FromDateTime(DateTime.Now))
        {
            throw new ArgumentException("Test date cannot be in the future");
        }

        // Validate number reached is positive
        if (request.NumberReached <= 0)
        {
            throw new ArgumentException("Number reached must be greater than 0");
        }
    }

    /// <inheritdoc />
    public async Task<bool> UpdateSpirometryEntryAsync(int entryId, SpirometryEntryUpdateRequest request)
    {
        _logger.LogDebug("Updating spirometry entry {EntryId}", entryId);

        // Retrieve the existing spirometry entry
        var entry = await _dbContext.SpirometryEntries
            .FirstOrDefaultAsync(e => e.Id == entryId);

        if (entry == null)
        {
            throw new KeyNotFoundException($"Spirometry entry with ID {entryId} not found");
        }

        // Validate test date is not in the future
        if (request.TestDate > DateOnly.FromDateTime(DateTime.Now))
        {
            throw new ArgumentException("Test date cannot be in the future");
        }

        // Validate number reached is positive
        if (request.NumberReached <= 0)
        {
            throw new ArgumentException("Number reached must be greater than 0");
        }

        // Update the entry using extension method
        entry.UpdateFrom(request);

        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully updated spirometry entry {EntryId}", entryId);

        return true;
    }

    /// <inheritdoc />
    public async Task<SpirometryEntryDetailResponse?> GetSpirometryEntryByIdAsync(int entryId)
    {
        _logger.LogDebug("Retrieving spirometry entry {EntryId}", entryId);

        var entry = await _dbContext.SpirometryEntries
            .FirstOrDefaultAsync(e => e.Id == entryId);

        if (entry == null)
        {
            throw new KeyNotFoundException($"Spirometry entry with ID {entryId} not found");
        }

        _logger.LogDebug("Successfully retrieved spirometry entry {EntryId}", entryId);

        return entry.ToDetailResponse();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SpirometryEntryDetailResponse>> GetSpirometryEntriesByPatientAsync(int patientId)
    {
        _logger.LogDebug("Retrieving all spirometry entries for patient {PatientId}", patientId);

        var entries = await _dbContext.SpirometryEntries
            .Where(e => e.PatientId == patientId)
            .OrderByDescending(e => e.TestDate)
            .ThenByDescending(e => e.TestTime)
            .ToListAsync();

        _logger.LogDebug("Retrieved {Count} spirometry entries for patient {PatientId}", entries.Count, patientId);

        return entries.Select(e => e.ToDetailResponse());
    }

    /// <inheritdoc />
    public async Task<SpirometryGraphResponse> GetSpirometryGraphAsync(SpirometryGraphRequest request)
    {
        _logger.LogDebug(
            "Retrieving spirometry graph data for patient {PatientId} for last {Days} days",
            request.PatientId,
            request.Days);

        // Get the patient's spirometry setup to retrieve capacity goal
        var spirometrySetup = await _dbContext.SpirometrySetups
            .FirstOrDefaultAsync(ss => ss.PatientId == request.PatientId);

        if (spirometrySetup == null)
        {
            throw new KeyNotFoundException($"No spirometry setup found for patient {request.PatientId}");
        }

        // Calculate date range
        var endDate = DateOnly.FromDateTime(DateTime.Now);
        var startDate = endDate.AddDays(-request.Days + 1);
        var today = DateOnly.FromDateTime(DateTime.Now);

        // Get all entries within the date range
        var entries = await _dbContext.SpirometryEntries
            .Where(e => e.PatientId == request.PatientId && e.TestDate >= startDate && e.TestDate <= endDate)
            .OrderBy(e => e.TestDate)
            .ThenBy(e => e.TestTime)
            .ToListAsync();

        // Group entries by date and get the latest entry for each day for the graph
        var dailyEntries = entries
            .GroupBy(e => e.TestDate)
            .Select(g => g.OrderByDescending(e => e.TestTime).First())
            .ToList();

        // Get today's entries (all entries for today, not just the latest)
        var todaysEntries = entries
            .Where(e => e.TestDate == today)
            .OrderByDescending(e => e.TestTime)
            .Select(e => e.ToDetailResponse())
            .ToList();

        // Build graph response
        var response = new SpirometryGraphResponse
        {
            StartDate = startDate,
            EndDate = endDate,
            CapacityGoal = spirometrySetup.CapacityGoal,
            ProviderInstructions = spirometrySetup.ProviderInstructions,
            TotalEntries = entries.Count,
            Values = dailyEntries.Select(e => new SpirometryValuePoint
            {
                Date = e.TestDate,
                NumberReached = e.NumberReached,
            }).ToList(),
            TodaysEntries = todaysEntries,
        };

        _logger.LogDebug(
            "Successfully generated spirometry graph data for patient {PatientId} with {EntryCount} entries",
            request.PatientId,
            response.TotalEntries);

        return response;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteSpirometryEntryAsync(int entryId)
    {
        _logger.LogDebug("Deleting spirometry entry {EntryId}", entryId);

        var entry = await _dbContext.SpirometryEntries
            .FirstOrDefaultAsync(e => e.Id == entryId);

        if (entry == null)
        {
            throw new KeyNotFoundException($"Spirometry entry with ID {entryId} not found");
        }

        // Hard delete for spirometry entries (no archiving)
        _dbContext.SpirometryEntries.Remove(entry);

        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully deleted spirometry entry {EntryId}", entryId);

        return true;
    }
}
