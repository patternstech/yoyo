using AH.CancerConnect.API.Features.Drainage.DrainageSetup;
using AH.CancerConnect.API.Features.Notes;
using AH.CancerConnect.API.Features.Questions;
using AH.CancerConnect.API.Features.Spirometry.SpirometrySetup;
using AH.CancerConnect.API.Features.SymptomsTracking.Models;
using AH.CancerConnect.API.Features.ToDo;

namespace AH.CancerConnect.API.SharedModels;

/// <summary>
/// Represents a patient in the Cancer Connect system.
/// </summary>
public class Patient
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string MychartLogin { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime Created { get; set; }

    // Navigation property
    public ICollection<SymptomEntry> SymptomEntries { get; set; } = new List<SymptomEntry>();

    public ICollection<Note> Notes { get; set; } = new List<Features.Notes.Note>();

    public ICollection<ToDo> ToDos { get; set; } = new List<Features.ToDo.ToDo>();

    public DrainageSetup? DrainageSetup { get; set; }

    public SpirometrySetup? SpirometrySetup { get; set; }

    public ICollection<Question> Questions { get; set; } = new List<Question>();
}
