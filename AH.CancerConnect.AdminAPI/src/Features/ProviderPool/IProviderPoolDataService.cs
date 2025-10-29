namespace AH.CancerConnect.AdminAPI.Features.ProviderPool;

/// <summary>
/// Interface for provider pool data operations.
/// </summary>
public interface IProviderPoolDataService
{
    /// <summary>
    /// Gets all provider pools.
    /// </summary>
    /// <returns>List of provider pools.</returns>
    Task<IEnumerable<ProviderPoolListResponse>> GetProviderPoolsAsync();

    /// <summary>
    /// Creates a new provider pool.
    /// </summary>
    /// <param name="request">The provider pool creation request.</param>
    /// <param name="createdBy">The username of the user creating the pool.</param>
    /// <returns>The ID of the created provider pool.</returns>
    Task<int> CreateProviderPoolAsync(ProviderPoolRequest request, string createdBy);

    /// <summary>
    /// Updates an existing provider pool.
    /// </summary>
    /// <param name="request">The provider pool update request.</param>
    /// <returns>The ID of the updated provider pool.</returns>
    Task<int> UpdateProviderPoolAsync(ProviderPoolUpdateRequest request);

    /// <summary>
    /// Deletes a provider pool by ID.
    /// </summary>
    /// <param name="id">The provider pool ID.</param>
    /// <returns>True if the provider pool was deleted successfully.</returns>
    Task<bool> DeleteProviderPoolAsync(int id);
}
