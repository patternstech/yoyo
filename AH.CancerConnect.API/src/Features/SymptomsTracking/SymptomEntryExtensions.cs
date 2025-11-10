using AH.CancerConnect.API.Features.SymptomsTracking.Models;

namespace AH.CancerConnect.API.Features.SymptomsTracking;

/// <summary>
/// Extension methods for mapping symptom entry requests to entities.
/// </summary>
public static class SymptomEntryExtensions
{
    /// <summary>
    /// Converts a SymptomEntryRequest to a SymptomEntry entity.
    /// </summary>
    /// <param name="request">The symptom entry request.</param>
    /// <returns>A new SymptomEntry entity.</returns>
    public static SymptomEntry ToEntity(this SymptomEntryRequest request)
    {
        return new SymptomEntry
        {
            PatientId = request.PatientId,
            Note = request.Note,
            EntryDate = request.EntryDate,
            Created = DateTime.Now,
        };
    }

    /// <summary>
    /// Updates an existing SymptomEntry entity from a SymptomEntryUpdateRequest.
    /// </summary>
    /// <param name="entry">The existing symptom entry entity.</param>
    /// <param name="request">The update request.</param>
    public static void UpdateFrom(this SymptomEntry entry, SymptomEntryUpdateRequest request)
    {
        entry.Note = request.Note;
        entry.EntryDate = request.EntryDate;
    }

    /// <summary>
    /// Converts a SymptomDetailRequest to a SymptomDetail entity.
    /// </summary>
    /// <param name="request">The symptom detail request.</param>
    /// <returns>A new SymptomDetail entity.</returns>
    public static SymptomDetail ToEntity(this SymptomDetailRequest request)
    {
        return new SymptomDetail
        {
            SymptomId = request.SymptomId,
            CategoryId = request.CategoryId,
            SymptomValue = request.SymptomValue,
        };
    }

    /// <summary>
    /// Updates an existing SymptomDetail entity from a SymptomDetailUpdateRequest.
    /// </summary>
    /// <param name="detail">The existing symptom detail entity.</param>
    /// <param name="request">The update request.</param>
    public static void UpdateFrom(this SymptomDetail detail, SymptomDetailUpdateRequest request)
    {
        detail.SymptomId = request.SymptomId;
        detail.CategoryId = request.CategoryId;
        detail.SymptomValue = request.SymptomValue;
    }

    /// <summary>
    /// Converts a SymptomDetailUpdateRequest to a SymptomDetail entity (for new details).
    /// </summary>
    /// <param name="request">The symptom detail update request.</param>
    /// <returns>A new SymptomDetail entity.</returns>
    public static SymptomDetail ToEntity(this SymptomDetailUpdateRequest request)
    {
        return new SymptomDetail
        {
            SymptomId = request.SymptomId,
            CategoryId = request.CategoryId,
            SymptomValue = request.SymptomValue,
        };
    }

    /// <summary>
    /// Converts a list of SymptomEntry entities to SymptomEntryDetailResponse objects.
    /// </summary>
    /// <param name="entries">The symptom entries to convert.</param>
    /// <returns>A list of SymptomEntryDetailResponse objects.</returns>
    public static List<SymptomEntryDetailResponse> ToSymptomEntryDetailResponses(this IEnumerable<SymptomEntry> entries)
    {
        return entries.Select(e => new SymptomEntryDetailResponse
        {
            Id = e.Id,
            PatientId = e.PatientId,
            Note = e.Note,
            EntryDate = e.EntryDate,
            Created = e.Created,
            Summary = string.Join(", ", e.SymptomDetails
                .Select(sd => sd.Symptom?.Name ?? string.Empty)
                .Where(name => !string.IsNullOrEmpty(name))
                .Distinct()),
            SymptomDetails = e.SymptomDetails.Select(sd => new SymptomDetailResponse
            {
                Id = sd.Id,
                SymptomId = sd.SymptomId,
                SymptomName = sd.Symptom?.Name ?? string.Empty,
                CategoryId = sd.CategoryId,
                CategoryName = sd.Category?.Name ?? string.Empty,
                SymptomValue = sd.SymptomValue,
            }).ToList(),
        }).ToList();
    }
}
