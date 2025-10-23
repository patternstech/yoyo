using System.ComponentModel.DataAnnotations;

namespace AH.CancerConnect.AdminAPI.Features.Provider;

/// <summary>
/// Provider entity for managing healthcare providers.
/// </summary>
public class Provider
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string ProviderId { get; set; } = string.Empty;

    public int? ProviderPoolId { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    // Navigation property
    public ProviderPool? ProviderPool { get; set; }
}

/// <summary>
/// Provider pool entity for organizing providers into care teams.
/// </summary>
public class ProviderPool
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    // Navigation properties
    public ICollection<Provider> Providers { get; set; } = new List<Provider>();
}

/// <summary>
/// Request model for creating a new provider.
/// </summary>
public class ProviderRequest
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, ErrorMessage = "First name must be 100 characters or less")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, ErrorMessage = "Last name must be 100 characters or less")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Provider ID is required")]
    [StringLength(50, ErrorMessage = "Provider ID must be 50 characters or less")]
    public string ProviderId { get; set; } = string.Empty;

    public int? ProviderPoolId { get; set; }

    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Request model for updating an existing provider.
/// </summary>
public class ProviderUpdateRequest
{
    /// <summary>
    /// Gets or sets the provider ID.
    /// </summary>
    [Required(ErrorMessage = "Provider ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Provider ID must be greater than 0")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the provider's first name.
    /// </summary>
    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, ErrorMessage = "First name must be 100 characters or less")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the provider's last name.
    /// </summary>
    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, ErrorMessage = "Last name must be 100 characters or less")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the provider's unique identifier.
    /// </summary>
    [Required(ErrorMessage = "Provider ID is required")]
    [StringLength(50, ErrorMessage = "Provider ID must be 50 characters or less")]
    public string ProviderId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the provider pool ID (optional).
    /// </summary>
    public int? ProviderPoolId { get; set; }

    /// <summary>
    /// Gets or sets whether the provider is active.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
