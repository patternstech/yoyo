namespace AH.CancerConnect.AdminAPI.Features.Provider;

/// <summary>
/// Response model for provider details.
/// </summary>
public class ProviderDetailResponse
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string ProviderId { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}".Trim();

    public int? ProviderPoolId { get; set; }

    public string? ProviderPoolName { get; set; }

    public bool IsActive { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }
}

/// <summary>
/// Response model for provider operations.
/// </summary>
public class ProviderResponse
{
    public int Id { get; set; }

    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// Response model for delete operations.
/// </summary>
public class DeleteProviderResponse
{
    public int Id { get; set; }

    public string Message { get; set; } = string.Empty;

    public bool Success { get; set; }
}