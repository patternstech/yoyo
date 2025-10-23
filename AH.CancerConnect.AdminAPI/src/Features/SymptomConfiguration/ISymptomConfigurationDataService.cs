namespace AH.CancerConnect.AdminAPI.Features.SymptomConfiguration;

/// <summary>
/// Interface for symptom configuration data operations.
/// </summary>
public interface ISymptomConfigurationDataService
{
    /// <summary>
    /// Gets all symptom configurations.
    /// </summary>
    /// <returns>List of symptom configurations.</returns>
    Task<IEnumerable<SymptomConfigurationResponse>> GetSymptomConfigurationsAsync();
}
