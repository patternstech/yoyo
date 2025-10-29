namespace AH.CancerConnect.AdminAPI.Features.ProviderPool;

/// <summary>
/// Response model for provider pool details.
/// </summary>
public class ProviderPoolDetailResponse
{
    public int Id { get; set; }

    public int ProviderPoolId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

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

    public int ProviderPoolId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime DateCreated { get; set; }

    public int ActiveProviderCount { get; set; }
}

/// <summary>
/// Response model for provider pool create/update operations.
/// </summary>
public class ProviderPoolResponse
{
    public bool Success { get; set; }

    public int Id { get; set; }
}
