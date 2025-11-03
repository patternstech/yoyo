namespace AH.CancerConnect.API.Features.Drainage.DraiangeEntry;

/// <summary>
/// Interface for drainage entry data service.
/// </summary>
public interface IDrainageEntryDataService
{
    /// <summary>
    /// Creates a new drainage entry.
    /// </summary>
    /// <param name="request">Drainage entry request.</param>
    /// <returns>ID of the created drainage entry.</returns>
    Task<int> CreateDrainageEntryAsync(DrainageEntryRequest request);
}