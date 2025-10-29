using AH.CancerConnect.AdminAPI.Features.Provider;

namespace AH.CancerConnect.AdminAPI.Features.ProviderPool;

/// <summary>
/// Extension methods for ProviderPool entities and requests.
/// </summary>
public static class ProviderPoolExtensions
{
    /// <summary>
    /// Converts a ProviderPool entity to a ProviderPoolListResponse.
    /// </summary>
    /// <param name="providerPool">The provider pool entity.</param>
    /// <returns>A provider pool list response.</returns>
    public static ProviderPoolListResponse ToListResponse(this Provider.ProviderPool providerPool)
    {
        return new ProviderPoolListResponse
        {
            Id = providerPool.Id,
            ProviderPoolId = providerPool.ProviderPoolId,
            Name = providerPool.Name,
            CreatedBy = providerPool.CreatedBy,
            DateCreated = providerPool.DateCreated,
            ActiveProviderCount = providerPool.Providers.Count,
        };
    }

    /// <summary>
    /// Converts a ProviderPool entity to a ProviderPoolDetailResponse.
    /// </summary>
    /// <param name="providerPool">The provider pool entity.</param>
    /// <returns>A provider pool detail response.</returns>
    public static ProviderPoolDetailResponse ToDetailResponse(this Provider.ProviderPool providerPool)
    {
        return new ProviderPoolDetailResponse
        {
            Id = providerPool.Id,
            ProviderPoolId = providerPool.ProviderPoolId,
            Name = providerPool.Name,
            Description = providerPool.Description,
            CreatedBy = providerPool.CreatedBy,
            DateCreated = providerPool.DateCreated,
            DateModified = providerPool.DateModified,
            ProviderCount = providerPool.Providers.Count,
            ActiveProviderCount = providerPool.Providers.Count,
        };
    }

    /// <summary>
    /// Converts a ProviderPoolRequest to a ProviderPool entity.
    /// </summary>
    /// <param name="request">The provider pool request.</param>
    /// <param name="createdBy">The username of the user creating the pool.</param>
    /// <returns>A new ProviderPool entity.</returns>
    public static Provider.ProviderPool ToEntity(this ProviderPoolRequest request, string createdBy)
    {
        return new Provider.ProviderPool
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            CreatedBy = createdBy,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
        };
    }

    /// <summary>
    /// Updates a ProviderPool entity from a ProviderPoolUpdateRequest.
    /// </summary>
    /// <param name="providerPool">The provider pool entity to update.</param>
    /// <param name="request">The update request.</param>
    public static void UpdateFrom(this Provider.ProviderPool providerPool, ProviderPoolUpdateRequest request)
    {
        providerPool.Name = request.Name.Trim();
        providerPool.Description = request.Description?.Trim();
        providerPool.DateModified = DateTime.UtcNow;
    }
}
