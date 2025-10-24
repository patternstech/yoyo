namespace AH.CancerConnect.AdminAPI.Features.Provider;

/// <summary>
/// Extension methods for Provider entities and requests.
/// </summary>
public static class ProviderExtensions
{
    /// <summary>
    /// Converts a ProviderPool entity to a ProviderPoolListResponse.
    /// </summary>
    /// <param name="providerPool">The provider pool entity.</param>
    /// <returns>A provider pool list response.</returns>
    public static ProviderPoolListResponse ToListResponse(this ProviderPool providerPool)
    {
        return new ProviderPoolListResponse
        {
            Id = providerPool.Id,
            Name = providerPool.Name,
            IsActive = providerPool.IsActive,
            ActiveProviderCount = providerPool.Providers.Count(p => p.IsActive),
        };
    }

    /// <summary>
    /// Converts a Provider entity to a ProviderDetailResponse.
    /// </summary>
    /// <param name="provider">The provider entity.</param>
    /// <returns>A provider detail response.</returns>
    public static ProviderDetailResponse ToDetailResponse(this Provider provider)
    {
        return new ProviderDetailResponse
        {
            Id = provider.Id,
            FirstName = provider.FirstName,
            LastName = provider.LastName,
            ProviderId = provider.ProviderId,
            Email = provider.Email,
            ProviderPoolId = provider.ProviderPoolId,
            ProviderPoolName = provider.ProviderPool?.Name,
            IsActive = provider.IsActive,
            DateCreated = provider.DateCreated,
            DateModified = provider.DateModified,
        };
    }

    /// <summary>
    /// Converts a ProviderRequest to a Provider entity.
    /// </summary>
    /// <param name="request">The provider request.</param>
    /// <returns>A new Provider entity.</returns>
    public static Provider ToEntity(this ProviderRequest request)
    {
        return new Provider
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            ProviderId = request.ProviderId.Trim(),
            Email = request.Email?.Trim(),
            ProviderPoolId = request.ProviderPoolId,
            IsActive = request.IsActive,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
        };
    }

    /// <summary>
    /// Updates a Provider entity from a ProviderUpdateRequest.
    /// </summary>
    /// <param name="provider">The provider entity to update.</param>
    /// <param name="request">The update request.</param>
    public static void UpdateFrom(this Provider provider, ProviderUpdateRequest request)
    {
        provider.FirstName = request.FirstName.Trim();
        provider.LastName = request.LastName.Trim();
        provider.ProviderId = request.ProviderId.Trim();
        provider.Email = request.Email?.Trim();
        provider.ProviderPoolId = request.ProviderPoolId;
        provider.IsActive = request.IsActive;
        provider.DateModified = DateTime.UtcNow;
    }
}