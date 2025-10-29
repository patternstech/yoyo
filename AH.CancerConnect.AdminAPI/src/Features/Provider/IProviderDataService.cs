namespace AH.CancerConnect.AdminAPI.Features.Provider;

/// <summary>
/// Interface for provider data operations.
/// </summary>
public interface IProviderDataService
{
    /// <summary>
    /// Gets all providers.
    /// </summary>
    /// <param name="includeInactive">Whether to include inactive providers.</param>
    /// <returns>List of providers.</returns>
    Task<IEnumerable<ProviderDetailResponse>> GetProvidersAsync();

    /// <summary>
    /// Creates a new provider.
    /// </summary>
    /// <param name="request">The provider creation request.</param>
    /// <returns>The ID of the created provider.</returns>
    Task<int> CreateProviderAsync(ProviderRequest request);

    /// <summary>
    /// Deletes a provider by ID.
    /// </summary>
    /// <param name="id">The provider ID.</param>
    /// <returns>True if the provider was deleted successfully.</returns>
    Task<bool> DeleteProviderAsync(int id);

    /// <summary>
    /// Updates an existing provider.
    /// </summary>
    /// <param name="request">The provider update request.</param>
    /// <returns>The ID of the updated provider.</returns>
    Task<int> UpdateProviderAsync(ProviderUpdateRequest request);
}