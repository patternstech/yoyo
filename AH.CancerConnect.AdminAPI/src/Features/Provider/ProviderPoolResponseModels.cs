namespace AH.CancerConnect.AdminAPI.Features.Provider;

/// <summary>
/// Response model for provider pool details.
/// </summary>
public class ProviderPoolDetailResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    public int ProviderCount { get; set; }

    public int ActiveProviderCount { get; set; }
}

/// <summary>
/// Response model for provider pool list/dropdown operations.
/// </summary>
public class ProviderPoolListResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public int ActiveProviderCount { get; set; }
}