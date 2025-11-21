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
        _logger.LogDebug("Creating drainage entry for patient {PatientId} with {DrainCount} drains", 
            request.PatientId, request.DrainEntries.Count);

        // Validate request
        await ValidateDrainageEntryRequest(request);

        // Create main drainage entry
        var drainageEntry = new DrainageEntry
        {
            PatientId = request.PatientId,
            EmptyDate = request.EmptyDate,
            Note = request.Note,
            IsArchived = false,
            DateCreated = DateTime.Now,
        };

        _dbContext.DrainageEntries.Add(drainageEntry);
        await _dbContext.SaveChangesAsync();

        // Create detail records for each drain
        foreach (var drainEntry in request.DrainEntries)
        {
            var detail = new DrainageEntryDetail
            {
                DrainageEntryId = drainageEntry.Id,
                DrainId = drainEntry.DrainId,
                Amount = drainEntry.Amount,
            };

            _dbContext.DrainageEntryDetails.Add(detail);
        }

        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully created drainage entry {EntryId} with {DetailCount} detail records", 
            drainageEntry.Id, request.DrainEntries.Count);

        return new List<int> { drainageEntry.Id };
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
        _logger.LogDebug("Updating drainage entry {DrainageEntryId}", request.DrainageEntryId);

        // Validate empty date is not in the future
        if (request.EmptyDate > DateTime.Now.AddDays(1))
        {
            throw new ArgumentException("Empty date cannot be in the future");
        }

        // Find the drainage entry
        var entry = await _dbContext.DrainageEntries
            .Include(e => e.DrainageEntryDetails)
            .FirstOrDefaultAsync(e => e.Id == request.DrainageEntryId);

        if (entry == null)
        {
            throw new KeyNotFoundException($"Drainage entry {request.DrainageEntryId} not found");
        }

        // Validate entry is not archived
        if (entry.IsArchived)
        {
            throw new InvalidOperationException($"Cannot update archived drainage entry {entry.Id}");
        }

        // Validate patient owns this entry
        if (entry.PatientId != request.PatientId)
        {
            throw new ArgumentException($"Drainage entry {request.DrainageEntryId} does not belong to patient {request.PatientId}");
        }

        // Get all drain IDs from the request
        var drainIds = request.DrainEntries.Select(e => e.DrainId).Distinct().ToList();

        // Validate all drains belong to this patient
        var drainageSetup = await _dbContext.DrainageSetups
            .Include(ds => ds.Drains)
            .FirstOrDefaultAsync(ds => ds.PatientId == request.PatientId);

        if (drainageSetup == null)
        {
            throw new KeyNotFoundException($"No drainage setup found for patient {request.PatientId}");
        }

        var patientDrainIds = drainageSetup.Drains.Where(d => !d.IsArchived).Select(d => d.Id).ToList();
        var invalidDrainIds = drainIds.Except(patientDrainIds).ToList();
        
        if (invalidDrainIds.Any())
        {
            throw new ArgumentException($"Drain IDs {string.Join(", ", invalidDrainIds)} do not belong to patient {request.PatientId} or are archived");
        }

        // Update main entry
        entry.EmptyDate = request.EmptyDate;
        entry.Note = request.Note;

        // Remove existing details
        _dbContext.DrainageEntryDetails.RemoveRange(entry.DrainageEntryDetails);

        // Add new details
        foreach (var updateItem in request.DrainEntries)
        {
            // Validate amount
            if (updateItem.Amount < 0 || updateItem.Amount > 100)
            {
                throw new ArgumentException("Drainage entry amount must be between 0 and 100 mL");
            }

            var detail = new DrainageEntryDetail
            {
                DrainageEntryId = entry.Id,
                DrainId = updateItem.DrainId,
                Amount = updateItem.Amount,
            };

            _dbContext.DrainageEntryDetails.Add(detail);
        }

        await _dbContext.SaveChangesAsync();

        _logger.LogDebug("Successfully updated drainage entry {DrainageEntryId}", request.DrainageEntryId);

        return true;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<DrainageSessionResponse>> GetDrainageSessionsByPatientAsync(int patientId)
    {
        _logger.LogDebug("Retrieving drainage sessions for patient {PatientId}", patientId);

        // Get all entries for the patient (non-archived only) with details
        var entries = await _dbContext.DrainageEntries
            .Include(e => e.DrainageEntryDetails)
                .ThenInclude(d => d.Drain)
            .Where(e => e.PatientId == patientId && !e.IsArchived)
            .OrderByDescending(e => e.EmptyDate)
            .ThenBy(e => e.DateCreated)
            .ToListAsync();

        // Map to response
        var sessions = entries.Select(e => new DrainageSessionResponse
        {
            DrainageEntryId = e.Id,
            PatientId = e.PatientId,
            EmptyDate = e.EmptyDate,
            Note = e.Note,
            DrainEntries = e.DrainageEntryDetails.Select(d => new DrainEntryDetail
            {
                DrainId = d.DrainId,
                Amount = d.Amount,
                DrainName = d.Drain.Name,
                IsArchived = d.Drain.IsArchived,
            }).ToList(),
        }).ToList();

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

    /// <inheritdoc />
    public async Task<DrainageGraphResponse> GetDrainageGraphAsync(DrainageGraphRequest request)
    {
        _logger.LogDebug(
            "Retrieving drainage graph for patient {PatientId} (excluding today's data)",
            request.PatientId);

        // Get all drains for the patient through drainage setup
        var drainageSetup = await _dbContext.DrainageSetups
            .Include(ds => ds.Drains)
            .FirstOrDefaultAsync(ds => ds.PatientId == request.PatientId);

        if (drainageSetup == null)
        {
            throw new KeyNotFoundException($"No drainage setup found for patient {request.PatientId}");
        }

        // Get all entries for the patient excluding today (non-archived only)
        var today = DateOnly.FromDateTime(DateTime.Now);
        var entries = await _dbContext.DrainageEntries
            .Include(e => e.DrainageEntryDetails)
            .Where(e => e.PatientId == request.PatientId
                     && !e.IsArchived
                     && DateOnly.FromDateTime(e.EmptyDate) < today)
            .OrderBy(e => e.EmptyDate)
            .ToListAsync();

        if (!entries.Any())
        {
            // No entries found - return empty response
            return new DrainageGraphResponse
            {
                TotalEntries = 0,
                Alert = DrainageAlert.NONE,
                DrainagesData = new List<DrainageDataPoint>(),
                TodayDrainageEntries = new List<DrainageSessionResponse>(),
            };
        }

        // Calculate total entries
        var totalEntries = entries.Count;

        // Group by date and sum amounts for graph data
        var graphData = entries
            .GroupBy(e => DateOnly.FromDateTime(e.EmptyDate))
            .Select(g => new DrainageDataPoint
            {
                Date = g.Key,
                Value = g.Sum(e => e.DrainageEntryDetails.Sum(d => d.Amount)),
            })
            .OrderBy(d => d.Date)
            .ToList();

        // Calculate alert level based on drainage conditions (today's entries already excluded)
        var alert = CalculateDrainageAlert(graphData, drainageSetup);

        // Get today's drainage sessions for display (not used in graph or alert calculation)
        var todayEntries = await _dbContext.DrainageEntries
            .Include(e => e.DrainageEntryDetails)
                .ThenInclude(d => d.Drain)
            .Where(e => e.PatientId == request.PatientId
                     && !e.IsArchived
                     && DateOnly.FromDateTime(e.EmptyDate) == today)
            .Select(e => new DrainageSessionResponse
            {
                DrainageEntryId = e.Id,
                PatientId = request.PatientId,
                EmptyDate = e.EmptyDate,
                Note = e.Note,
                DrainEntries = e.DrainageEntryDetails.Select(d => new DrainEntryDetail
                {
                    DrainId = d.DrainId,
                    Amount = d.Amount,
                    DrainName = d.Drain.Name,
                    IsArchived = d.Drain.IsArchived,
                }).ToList(),
            })
            .OrderByDescending(s => s.EmptyDate)
            .ToListAsync();

        var response = new DrainageGraphResponse
        {
            TotalEntries = totalEntries,
            Alert = alert,
            DrainagesData = graphData,
            TodayDrainageEntries = todayEntries,
        };

        _logger.LogDebug(
            "Retrieved drainage graph with {TotalEntries} entries and {DataPoints} data points for patient {PatientId}",
            totalEntries,
            graphData.Count,
            request.PatientId);

        return response;
    }

    private DrainageAlert CalculateDrainageAlert(
        List<DrainageDataPoint> dailyTotals,
        DrainageSetup.DrainageSetup drainageSetup)
    {
        if (!dailyTotals.Any())
        {
            return DrainageAlert.NONE;
        }

        // Order by date to check conditions
        var orderedData = dailyTotals.OrderBy(d => d.Date).ToList();

        // Determine goal threshold (use provider goal or default 30 mL)
        var goalThreshold = drainageSetup.HasProviderGoalAmount && drainageSetup.GoalDrainageAmount.HasValue
            ? drainageSetup.GoalDrainageAmount.Value
            : 30m;

        // Check for GOAL_REACHED (highest priority)
        // Must have at least 2 days of data to check for 2 consecutive days at or below goal
        if (orderedData.Count >= 2)
        {
            // Check if the last two consecutive days are at or below the goal
            var secondToLast = orderedData[orderedData.Count - 2];
            var last = orderedData[orderedData.Count - 1];

            if (secondToLast.Value <= goalThreshold && last.Value <= goalThreshold)
            {
                return DrainageAlert.GOAL_REACHED;
            }
        }

        // Check for LARGE_INCREASE (more than 50 mL in a single day)
        if (orderedData.Count >= 2)
        {
            for (int i = 1; i < orderedData.Count; i++)
            {
                var previousDay = orderedData[i - 1];
                var currentDay = orderedData[i];
                var increase = currentDay.Value - previousDay.Value;

                if (increase > 50)
                {
                    return DrainageAlert.LARGE_INCREASE;
                }
            }
        }

        // Check for TWO_CONSECUTIVE_DAYS_INCREASED
        if (orderedData.Count >= 3)
        {
            for (int i = 2; i < orderedData.Count; i++)
            {
                var day1 = orderedData[i - 2];
                var day2 = orderedData[i - 1];
                var day3 = orderedData[i];

                // Check if day2 > day1 AND day3 > day2
                if (day2.Value > day1.Value && day3.Value > day2.Value)
                {
                    return DrainageAlert.TWO_CONSECUTIVE_DAYS_INCREASED;
                }
            }
        }

        return DrainageAlert.NONE;
    }
}
