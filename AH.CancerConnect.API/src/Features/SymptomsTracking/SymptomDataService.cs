using AH.CancerConnect.API.Features.SymptomsTracking.Models;
using Microsoft.EntityFrameworkCore;

namespace AH.CancerConnect.API.Features.SymptomsTracking;

/// <summary>
/// Database implementation of symptom data service.
/// </summary>
public class SymptomDataService : ISymptomDataService
{
    private readonly CancerConnectDbContext _dbContext;
    private readonly ILogger<SymptomDataService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SymptomDataService"/> class.
    /// </summary>
    /// <param name="dbContext">Database context.</param>
    /// <param name="logger">Logger instance.</param>
    public SymptomDataService(CancerConnectDbContext dbContext, ILogger<SymptomDataService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SymptomResponse>> GetSymptomsAsync()
    {
        _logger.LogInformation("Retrieving symptoms from database with severity options");

        var symptoms = await _dbContext.Symptoms
            .Where(s => !s.Invalid)
            .ToListAsync();

        var response = new List<SymptomResponse>();

        foreach (var symptom in symptoms)
        {
            var symptomResponse = new SymptomResponse
            {
                Id = symptom.Id,
                Name = symptom.Name,
                DisplayTitle = symptom.DisplayTitle,
                Description = symptom.Description,
            };

            // Get available symptom ranges for this symptom
            var ranges = await _dbContext.SymptomRanges
                .Include(sr => sr.Category)
                .Where(sr => sr.SymptomId == symptom.Id)
                .ToListAsync();


            // Since each symptom should have only one category, get the first one
            var firstRange = ranges.FirstOrDefault();
            if (firstRange != null)
            {
                symptomResponse.CategoryId = firstRange.Category.Id;
                symptomResponse.AvailableValues = ranges.Select(r => r.SymptomValue).ToList();
            }
            else
            {
                // Fallback if no ranges are found
                symptomResponse.CategoryId = 0;
                symptomResponse.AvailableValues = new List<string>();
            }

            response.Add(symptomResponse);
        }

        _logger.LogInformation(
            "Retrieved {SymptomCount} symptoms from database with severity options",
            symptoms.Count);

        return response;
    }

    /// <inheritdoc />
    public async Task<int> CreateSymptomEntryAsync(SymptomEntryRequest request)
    {
        _logger.LogInformation(
            "Creating symptom entry for patient {PatientId} with {Count} details",
            request.PatientId,
            request.SymptomDetails.Count);

        // Validate all symptom details before creating
        await ValidateSymptomDetailsAsync(request.SymptomDetails);

        // Create the entry using
        var entry = request.ToEntity();

        // Add all symptom details
        AddSymptomDetailsToEntry(entry, request.SymptomDetails);

        // Save to database
        _dbContext.SymptomEntries.Add(entry);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation(
            "Successfully created entry {EntryId} for patient {PatientId} with {Count} details",
            entry.Id,
            request.PatientId,
            entry.SymptomDetails.Count);

        return entry.Id;
    }

    /// <inheritdoc />
    public async Task<int> UpdateSymptomEntryAsync(SymptomEntryUpdateRequest request)
    {
        _logger.LogInformation(
            "Updating symptom entry {EntryId} for patient {PatientId} with {Count} details",
            request.Id,
            request.PatientId,
            request.SymptomDetails.Count);

        // Retrieve the existing entry with its details - EF Core will track this entity
        var entry = await _dbContext.SymptomEntries
            .Include(e => e.SymptomDetails)
            .FirstOrDefaultAsync(e => e.Id == request.Id);

        if (entry == null)
        {
            throw new KeyNotFoundException($"Symptom entry with ID {request.Id} not found.");
        }

        // Verify the entry belongs to the specified patient
        if (entry.PatientId != request.PatientId)
        {
            throw new ArgumentException($"Symptom entry {request.Id} does not belong to patient {request.PatientId}.");
        }

        // Validate all symptom details before making any changes
        await ValidateSymptomDetailsAsync(request.SymptomDetails);

        // Update entry properties using
        entry.UpdateFrom(request);

        // Update symptom details collection
        UpdateSymptomDetails(entry, request.SymptomDetails);

        // Save all tracked changes
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation(
            "Successfully updated entry {EntryId} for patient {PatientId} with {Count} details",
            entry.Id,
            request.PatientId,
            entry.SymptomDetails.Count);

        return entry.Id;
    }

    public async Task<bool> DeleteSymptomEntryAsync(int entryId, int patientId)
    {
        _logger.LogDebug("Deleting symptom entry {EntryId} for patient {PatientId}", entryId, patientId);

        // Retrieve the existing entry with its details
        var entry = await _dbContext.SymptomEntries
            .Include(e => e.SymptomDetails)
            .FirstOrDefaultAsync(e => e.Id == entryId);

        if (entry == null)
        {
            throw new KeyNotFoundException($"Symptom entry with ID {entryId} not found.");
        }

        // Verify the entry belongs to the specified patient
        if (entry.PatientId != patientId)
        {
            throw new ArgumentException($"Symptom entry {entryId} does not belong to patient {patientId}.");
        }

        // Remove the entry EF will cascade delete the details
        _dbContext.SymptomEntries.Remove(entry);
        await _dbContext.SaveChangesAsync();

        _logger.LogDebug(
            "Successfully deleted entry {EntryId} for patient {PatientId} with {Count} details",
            entryId,
            patientId,
            entry.SymptomDetails.Count);

        return true;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SymptomEntryDetailResponse>> GetSymptomSummaryAsync(int patientId)
    {
        _logger.LogDebug("Retrieving symptom summary for patient {PatientId}", patientId);

        // Validate patient exists
        var patientExists = await _dbContext.Patients
            .AnyAsync(p => p.Id == patientId);

        if (!patientExists)
        {
            throw new ArgumentException($"Patient with ID {patientId} not found.");
        }

        // Get all symptom entries for the patient with their details and related data
        var entries = await _dbContext.SymptomEntries
            .Where(e => e.PatientId == patientId)
            .Include(e => e.SymptomDetails)
                .ThenInclude(sd => sd.Symptom)
            .Include(e => e.SymptomDetails)
                .ThenInclude(sd => sd.Category)
            .OrderByDescending(e => e.EntryDate)
            .ThenByDescending(e => e.Created)
            .ToListAsync();

        var response = entries.ToSymptomEntryDetailResponses();

        _logger.LogDebug(
            "Retrieved symptom summary for patient {PatientId} with {EntryCount} entries",
            patientId,
            entries.Count);

        return response;
    }

    /// <summary>
    /// Validates that a symptom/category/value combination exists in SymptomRanges.
    /// </summary>
    private async Task ValidateSymptomDetailAsync(int symptomId, int categoryId, string symptomValue)
    {
        var validRange = await _dbContext.SymptomRanges
            .AnyAsync(sr => sr.SymptomId == symptomId &&
                           sr.CategoryId == categoryId &&
                           sr.SymptomValue == symptomValue);

        if (!validRange)
        {
            throw new ArgumentException(
                $"Invalid combination: SymptomId {symptomId}, CategoryId {categoryId}, Value '{symptomValue}' not found in valid ranges.");
        }
    }

    /// <summary>
    /// Validates all symptom details in a request.
    /// </summary>
    private async Task ValidateSymptomDetailsAsync<T>(List<T> details)
        where T : class
    {
        foreach (var detail in details)
        {
            var (symptomId, categoryId, symptomValue) = detail switch
            {
                SymptomDetailRequest req => (req.SymptomId, req.CategoryId, req.SymptomValue),
                SymptomDetailUpdateRequest req => (req.SymptomId, req.CategoryId, req.SymptomValue),
                _ => throw new InvalidOperationException("Unsupported detail type")
            };

            await ValidateSymptomDetailAsync(symptomId, categoryId, symptomValue);
        }
    }

    /// <summary>
    /// Adds symptom details to an entry.
    /// </summary>
    private void AddSymptomDetailsToEntry(SymptomEntry entry, IEnumerable<SymptomDetailRequest> detailRequests)
    {
        foreach (var detailRequest in detailRequests)
        {
            entry.SymptomDetails.Add(detailRequest.ToEntity());
        }
    }

    /// <summary>
    /// Updates symptom details collection for an existing entry.
    /// </summary>
    private void UpdateSymptomDetails(SymptomEntry entry, List<SymptomDetailUpdateRequest> detailRequests)
    {
        // First, validate that all provided IDs actually exist in the entry
        var existingDetailIds = entry.SymptomDetails.Select(d => d.Id).ToHashSet();
        var requestedIdsToUpdate = detailRequests
            .Where(d => d.Id.HasValue)
            .Select(d => d.Id!.Value)
            .ToList();

        // Check for invalid IDs (IDs that don't exist in the current entry)
        var invalidIds = requestedIdsToUpdate.Where(id => !existingDetailIds.Contains(id)).ToList();
        if (invalidIds.Any())
        {
            throw new ArgumentException(
                $"Invalid detail ID(s): {string.Join(", ", invalidIds)}. These details do not belong to entry {entry.Id}.");
        }

        // Get IDs of details to keep (valid IDs from request)
        var requestDetailIds = requestedIdsToUpdate.ToHashSet();

        // Remove details not in the request
        var detailsToRemove = entry.SymptomDetails
            .Where(d => !requestDetailIds.Contains(d.Id))
            .ToList();

        foreach (var detail in detailsToRemove)
        {
            entry.SymptomDetails.Remove(detail);
        }

        // Process each detail in the request
        foreach (var detailRequest in detailRequests)
        {
            if (detailRequest.Id.HasValue)
            {
                // Update existing detail (we already validated it exists)
                var existingDetail = entry.SymptomDetails.First(d => d.Id == detailRequest.Id.Value);
                existingDetail.UpdateFrom(detailRequest);
            }
            else
            {
                // Add new detail
                entry.SymptomDetails.Add(detailRequest.ToEntity());
            }
        }
    }

    /// <inheritdoc />
    public async Task<SymptomGraphResponse> GetSymptomGraphDataAsync(int patientId, int days)
    {
        _logger.LogInformation("Retrieving symptom graph data for patient {PatientId} for {Days} days", patientId, days);

        var endDate = DateTime.Now.Date.AddDays(1); // End of today (tomorrow 00:00)
        var startDate = DateTime.Now.Date.AddDays(-days); // Go back 'days' from today

        _logger.LogInformation("Date range: {StartDate} to {EndDate}", startDate, endDate);

        var entries = await _dbContext.SymptomEntries
            .Include(e => e.SymptomDetails)
                .ThenInclude(sd => sd.Symptom)
            .Include(e => e.SymptomDetails)
                .ThenInclude(sd => sd.Category)
            .Where(e => e.PatientId == patientId
                && e.EntryDate >= startDate
                && e.EntryDate < endDate)
            .OrderBy(e => e.EntryDate)
            .ToListAsync();

        _logger.LogInformation("Found {Count} entries for patient {PatientId}", entries.Count, patientId);


        // Group symptoms by name
        var symptomGroups = entries
            .SelectMany(e => e.SymptomDetails)
            .GroupBy(sd => new { sd.Symptom.Name, sd.CategoryId })
            .ToList();

        var symptomsData = new List<SymptomGraphData>();

        foreach (var symptomGroup in symptomGroups)
        {
            var symptomData = new SymptomGraphData
            {
                Name = symptomGroup.Key.Name.ToLowerInvariant(),
                CategoryId = symptomGroup.Key.CategoryId,
                Values = new List<SymptomValuePoint>(),
            };

            // Get all values for this symptom, ordered by date
            var symptomValues = symptomGroup
                .Select(sd => new
                {
                    Date = sd.SymptomEntry.EntryDate,
                    Value = ConvertSymptomValueToGraphValue(sd.SymptomValue, sd.Category.Name),
                    CategoryName = sd.Category.Name
                })
                .OrderBy(sv => sv.Date)
                .ToList();

            foreach (var value in symptomValues)
            {
                symptomData.Values.Add(new SymptomValuePoint
                {
                    Date = value.Date.Date,
                    Value = value.Value
                });
            }

            if (symptomData.Values.Any())
            {
                symptomsData.Add(symptomData);
            }
        }

        // Calculate summary statistics
        var daysWithEntries = entries.Select(e => e.EntryDate.Date).Distinct().Count();
        var uniqueSymptoms = symptomGroups.Count();

        // Get total number of entries for this patient (regardless of date range)
        var totalEntries = await _dbContext.SymptomEntries
            .Where(e => e.PatientId == patientId)
            .CountAsync();

        var response = new SymptomGraphResponse
        {
            StartDate = startDate.Date,
            EndDate = endDate.Date,
            DaysWithSymptoms = daysWithEntries,
            SymptomsTracked = uniqueSymptoms,
            TotalEntries = totalEntries,
            SymptomsData = symptomsData.OrderBy(s => s.Name).ToList()
        };

        _logger.LogInformation("Retrieved graph data for {DaysWithSymptoms} days and {SymptomsTracked} symptoms", 
            daysWithEntries, uniqueSymptoms);

        return response;
    }

    /// <summary>
    /// Converts symptom value to standardized graph value based on category.
    /// </summary>
    /// <param name="symptomValue">Original symptom value.</param>
    /// <param name="categoryName">Category name to determine conversion logic.</param>
    /// <returns>Standardized value for graphing.</returns>
    private static string ConvertSymptomValueToGraphValue(string symptomValue, string categoryName)
    {
        return categoryName.ToLowerInvariant() switch
        {
            "yesno" => symptomValue.ToLowerInvariant() switch
            {
                "yes" => "1",
                "no" => "2",
                _ => symptomValue
            },
            "mildmoderatesevere" => symptomValue.ToLowerInvariant() switch
            {
                "mild" => "1",
                "moderate" => "2",
                "severe" => "3",
                _ => symptomValue
            },
            "scale1to10" => symptomValue,
            _ => symptomValue
        };
    }
}
