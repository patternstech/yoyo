namespace AH.CancerConnect.API.Features.SymptomsTracking.Models;

/// <summary>
/// Defines the types of severity scales available for symptoms.
/// </summary>
public enum SeverityType
{
    /// <summary>
    /// Mild, Moderate, Severe scale.
    /// </summary>
    MildModerateSevere,

    /// <summary>
    /// Yes/No scale.
    /// </summary>
    YesNo,

    /// <summary>
    /// 1 to 10 numeric scale.
    /// </summary>
    Scale1To10,
}