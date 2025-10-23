namespace AH.CancerConnect.API.Features.SymptomsTracking.Models;

/// <summary>
/// Represents a symptom with its available severity options.
/// </summary>
public class SymptomWithSeverityOptions
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public SeverityType SeverityType { get; set; }

    public List<string> SeverityOptions { get; set; } = new ();
}
