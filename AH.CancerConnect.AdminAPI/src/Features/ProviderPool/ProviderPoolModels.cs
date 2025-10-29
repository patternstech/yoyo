using System.ComponentModel.DataAnnotations;

namespace AH.CancerConnect.AdminAPI.Features.ProviderPool;

/// <summary>
/// Request model for creating a new provider pool.
/// </summary>
public class ProviderPoolRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, ErrorMessage = "Name must be 200 characters or less")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description must be 500 characters or less")]
    public string? Description { get; set; }
}

/// <summary>
/// Request model for updating an existing provider pool.
/// </summary>
public class ProviderPoolUpdateRequest
{
    [Required(ErrorMessage = "Provider Pool ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Provider Pool ID must be greater than 0")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, ErrorMessage = "Name must be 200 characters or less")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description must be 500 characters or less")]
    public string? Description { get; set; }
}
