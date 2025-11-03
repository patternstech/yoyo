using AH.CancerConnect.API.Features.Drainage.DraiangeEntry;
using AH.CancerConnect.API.Features.Drainage.DrainageSetup;
using AH.CancerConnect.API.Features.Notes;
using AH.CancerConnect.API.Features.SymptomsTracking.Models;
using AH.CancerConnect.API.Features.ToDo;
using AH.CancerConnect.API.SharedModels;
using Microsoft.EntityFrameworkCore;

namespace AH.CancerConnect.API;

/// <summary>
/// Database context for Cancer Connect application.
/// </summary>
public class CancerConnectDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CancerConnectDbContext"/> class.
    /// </summary>
    /// <param name="options">Database context options.</param>
    public CancerConnectDbContext(DbContextOptions<CancerConnectDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the patients table.
    /// </summary>
    public DbSet<Patient> Patients { get; set; }

    /// <summary>
    /// Gets or sets the symptoms table.
    /// </summary>
    public DbSet<Symptom> Symptoms { get; set; }

    /// <summary>
    /// Gets or sets the symptom categories table.
    /// </summary>
    public DbSet<SymptomCategory> SymptomCategories { get; set; }

    /// <summary>
    /// Gets or sets the symptom ranges table.
    /// </summary>
    public DbSet<SymptomRange> SymptomRanges { get; set; }

    /// <summary>
    /// Gets or sets the symptom entries table.
    /// </summary>
    public DbSet<SymptomEntry> SymptomEntries { get; set; }

    /// <summary>
    /// Gets or sets the symptom details table.
    /// </summary>
    public DbSet<SymptomDetail> SymptomDetails { get; set; }

    /// <summary>
    /// Gets or sets the notes table.
    /// </summary>
    public DbSet<Note> Notes { get; set; }

    /// <summary>
    /// Gets or sets the todo items table.
    /// </summary>
    public DbSet<ToDo> ToDos { get; set; }

    /// <summary>
    /// Gets or sets the drainage setups table.
    /// </summary>
    public DbSet<DrainageSetup> DrainageSetups { get; set; }

    /// <summary>
    /// Gets or sets the drains table.
    /// </summary>
    public DbSet<Drain> Drains { get; set; }

    /// <summary>
    /// Gets or sets the drainage entries table.
    /// </summary>
    public DbSet<DrainageEntry> DrainageEntries { get; set; }

    /// <summary>
    /// Configures the model relationships and constraints.
    /// </summary>
    /// <param name="modelBuilder">Model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure table names
        modelBuilder.Entity<Patient>().ToTable("Patient", "dbo");
        modelBuilder.Entity<Symptom>().ToTable("Symptom", "ref");
        modelBuilder.Entity<SymptomCategory>().ToTable("SymptomCategory", "ref");
        modelBuilder.Entity<SymptomRange>().ToTable("SymptomRange", "ref");
        modelBuilder.Entity<SymptomEntry>().ToTable("SymptomEntry", "dbo");
        modelBuilder.Entity<SymptomDetail>().ToTable("SymptomDetail", "dbo");
        modelBuilder.Entity<Note>().ToTable("Note", "dbo");
        modelBuilder.Entity<ToDo>().ToTable("ToDo", "dbo");
        modelBuilder.Entity<DrainageSetup>().ToTable("DrainageSetup", "dbo");
        modelBuilder.Entity<Drain>().ToTable("Drain", "dbo");
        modelBuilder.Entity<DrainageEntry>().ToTable("DrainageEntry", "dbo");

        // Configure Patient entity
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.FirstName).IsRequired().HasMaxLength(250);
            entity.Property(p => p.LastName).IsRequired().HasMaxLength(250);
            entity.Property(p => p.MychartLogin).IsRequired().HasMaxLength(250);
            entity.Property(p => p.Status).IsRequired().HasMaxLength(250);
            entity.Property(p => p.Created).IsRequired();
        });

        // Configure Symptom entity (reference table)
        modelBuilder.Entity<Symptom>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(250);
            entity.Property(s => s.DisplayTitle).IsRequired().HasMaxLength(250);
            entity.Property(s => s.Description).IsRequired().HasMaxLength(500);
            entity.Property(s => s.Invalid).IsRequired();
        });

        // Configure SymptomCategory entity (reference table)
        modelBuilder.Entity<SymptomCategory>(entity =>
        {
            entity.HasKey(sc => sc.Id);
            entity.Property(sc => sc.Name).IsRequired().HasMaxLength(250);
            entity.Property(sc => sc.DisplayValue).IsRequired().HasMaxLength(250);
        });

        // Configure SymptomRange entity (reference table)
        modelBuilder.Entity<SymptomRange>(entity =>
        {
            entity.HasKey(sr => sr.Id);
            entity.Property(sr => sr.SymptomId).IsRequired();
            entity.Property(sr => sr.CategoryId).IsRequired();
            entity.Property(sr => sr.SymptomValue).IsRequired().HasMaxLength(255);

            // Relationships
            entity.HasOne(sr => sr.Symptom)
                  .WithMany(s => s.SymptomRanges)
                  .HasForeignKey(sr => sr.SymptomId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(sr => sr.Category)
                  .WithMany(sc => sc.SymptomRanges)
                  .HasForeignKey(sr => sr.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Configure Note entity
            modelBuilder.Entity<Note>(entity =>
            {
                entity.HasKey(n => n.Id);
                entity.Property(n => n.PatientId).IsRequired();
                entity.Property(n => n.Title).HasMaxLength(250);
                entity.Property(n => n.NoteText).IsRequired().HasMaxLength(1000);
                entity.Property(n => n.DateCreated).IsRequired();

                // Relationship with Patient
                entity.HasOne(n => n.Patient)
                      .WithMany(p => p.Notes)
                      .HasForeignKey(n => n.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        });

        // Configure SymptomEntry entity
        modelBuilder.Entity<SymptomEntry>(entity =>
        {
            entity.HasKey(se => se.Id);
            entity.Property(se => se.PatientId).IsRequired();
            entity.Property(se => se.Note).IsRequired().HasMaxLength(2000);
            entity.Property(se => se.EntryDate).IsRequired();
            entity.Property(se => se.Created).IsRequired();

            // Relationship with Patient
            entity.HasOne(se => se.Patient)
                  .WithMany(p => p.SymptomEntries)
                  .HasForeignKey(se => se.PatientId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure SymptomDetail entity
        modelBuilder.Entity<SymptomDetail>(entity =>
        {
            entity.HasKey(sd => sd.Id);
            entity.Property(sd => sd.SymptomEntryId).IsRequired();
            entity.Property(sd => sd.SymptomId).IsRequired();
            entity.Property(sd => sd.CategoryId).IsRequired();
            entity.Property(sd => sd.SymptomValue).IsRequired().HasMaxLength(255);

            // Relationships
            entity.HasOne(sd => sd.SymptomEntry)
                  .WithMany(se => se.SymptomDetails)
                  .HasForeignKey(sd => sd.SymptomEntryId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(sd => sd.Symptom)
                  .WithMany(s => s.SymptomDetails)
                  .HasForeignKey(sd => sd.SymptomId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(sd => sd.Category)
                  .WithMany(sc => sc.SymptomDetails)
                  .HasForeignKey(sd => sd.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ToDo entity
        modelBuilder.Entity<ToDo>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.PatientId).IsRequired();
            entity.Property(t => t.Title).HasMaxLength(250);
            entity.Property(t => t.Detail).IsRequired().HasMaxLength(1000);
            entity.Property(t => t.Date);
            entity.Property(t => t.Time);
            entity.Property(t => t.Alert).HasConversion<string>();
            entity.Property(t => t.IsCompleted).IsRequired().HasDefaultValue(false);
            entity.Property(t => t.DateCreated).IsRequired();

            // Relationship with Patient
            entity.HasOne(t => t.Patient)
                  .WithMany(p => p.ToDos)
                  .HasForeignKey(t => t.PatientId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure DrainageSetup entity
        modelBuilder.Entity<DrainageSetup>(entity =>
        {
            entity.HasKey(ds => ds.Id);
            entity.Property(ds => ds.PatientId).IsRequired();
            entity.Property(ds => ds.HasProviderGoalAmount).IsRequired();
            entity.Property(ds => ds.GoalDrainageAmount);
            entity.Property(ds => ds.ProviderInstructions).HasMaxLength(1000);
            entity.Property(ds => ds.DateCreated).IsRequired();
            entity.Property(ds => ds.DateModified).IsRequired();

            // Relationship with Patient (one-to-one)
            entity.HasOne(ds => ds.Patient)
                  .WithOne(p => p.DrainageSetup)
                  .HasForeignKey<DrainageSetup>(ds => ds.PatientId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Index for unique patient constraint
            entity.HasIndex(ds => ds.PatientId).IsUnique();
        });

        // Configure Drain entity
        modelBuilder.Entity<Drain>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.DrainageSetupId).IsRequired();
            entity.Property(d => d.Name).IsRequired().HasMaxLength(100);
            entity.Property(d => d.IsArchived).IsRequired().HasDefaultValue(false);
            entity.Property(d => d.DateCreated).IsRequired();
            entity.Property(d => d.DateArchived);

            // Relationship with DrainageSetup
            entity.HasOne(d => d.DrainageSetup)
                  .WithMany(ds => ds.Drains)
                  .HasForeignKey(d => d.DrainageSetupId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Index for performance
            entity.HasIndex(d => new { d.DrainageSetupId, d.IsArchived });
        });

        // Configure DrainageEntry entity
        modelBuilder.Entity<DrainageEntry>(entity =>
        {
            entity.HasKey(de => de.Id);
            entity.Property(de => de.DrainageSetupId).IsRequired();
            entity.Property(de => de.EmptyDate).IsRequired();
            entity.Property(de => de.Drain1Amount).HasColumnType("decimal(7,2)");
            entity.Property(de => de.Drain2Amount).HasColumnType("decimal(7,2)");
            entity.Property(de => de.Drain3Amount).HasColumnType("decimal(7,2)");
            entity.Property(de => de.Drain4Amount).HasColumnType("decimal(7,2)");
            entity.Property(de => de.Note).HasMaxLength(1000);
            entity.Property(de => de.IsArchived).IsRequired().HasDefaultValue(false);
            entity.Property(de => de.DateCreated).IsRequired();
            entity.Property(de => de.DateArchived);

            // Relationship with DrainageSetup
            entity.HasOne(de => de.DrainageSetup)
                  .WithMany()
                  .HasForeignKey(de => de.DrainageSetupId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Indexes for performance
            entity.HasIndex(de => new { de.DrainageSetupId, de.EmptyDate });
            entity.HasIndex(de => new { de.DrainageSetupId, de.IsArchived });
        });

        // Seed initial data
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Seed test patient
        var patients = new[]
        {
            new Patient { Id = 1, FirstName = "Test", LastName = "User", MychartLogin = "testuser", Status = "Active", Created = DateTime.UtcNow },
        };

        modelBuilder.Entity<Patient>().HasData(patients);

        // Seed symptoms (reference table)
        var symptoms = new[]
        {
            new Symptom { Id = 1, Name = "Anxiety", DisplayTitle = "Anxiety", Description = "Patient anxiety levels", Invalid = false },
            new Symptom { Id = 2, Name = "Appetite Loss", DisplayTitle = "Appetite Loss", Description = "Loss of appetite", Invalid = false },
            new Symptom { Id = 3, Name = "Bleeding", DisplayTitle = "Bleeding", Description = "Any bleeding symptoms", Invalid = false },
            new Symptom { Id = 4, Name = "Impaired activity (ADLs)", DisplayTitle = "Impaired Activities", Description = "Difficulty with daily activities", Invalid = false },
            new Symptom { Id = 5, Name = "Constipation", DisplayTitle = "Constipation", Description = "Bowel movement difficulties", Invalid = false },
            new Symptom { Id = 6, Name = "Cough", DisplayTitle = "Cough", Description = "Persistent coughing", Invalid = false },
            new Symptom { Id = 7, Name = "Depression", DisplayTitle = "Depression", Description = "Emotional depression", Invalid = false },
            new Symptom { Id = 8, Name = "Diarrhea", DisplayTitle = "Diarrhea", Description = "Loose bowel movements", Invalid = false },
            new Symptom { Id = 9, Name = "Fatigue", DisplayTitle = "Fatigue", Description = "Tiredness and low energy", Invalid = false },
            new Symptom { Id = 10, Name = "Fever", DisplayTitle = "Fever", Description = "Elevated body temperature", Invalid = false },
            new Symptom { Id = 11, Name = "Nausea or Vomiting", DisplayTitle = "Nausea/Vomiting", Description = "Feeling sick or vomiting", Invalid = false },
            new Symptom { Id = 12, Name = "Pain", DisplayTitle = "Pain", Description = "Physical pain levels", Invalid = false },
            new Symptom { Id = 13, Name = "Tingling and Numbness in hands or feet", DisplayTitle = "Tingling/Numbness", Description = "Peripheral neuropathy symptoms", Invalid = false },
            new Symptom { Id = 14, Name = "Shortness of breath", DisplayTitle = "Shortness of Breath", Description = "Difficulty breathing", Invalid = false },
            new Symptom { Id = 15, Name = "Skin rash", DisplayTitle = "Skin Rash", Description = "Skin irritation or rash", Invalid = false },
            new Symptom { Id = 16, Name = "Redness of skin", DisplayTitle = "Skin Redness", Description = "Red skin conditions", Invalid = false },
            new Symptom { Id = 17, Name = "Edema/Swelling", DisplayTitle = "Swelling", Description = "Fluid retention and swelling", Invalid = false },
            new Symptom { Id = 18, Name = "Mouth Sores", DisplayTitle = "Mouth Sores", Description = "Oral cavity sores", Invalid = false },
            new Symptom { Id = 19, Name = "Level of Emotional Distress", DisplayTitle = "Emotional Distress", Description = "Overall emotional distress level", Invalid = false },
        };

        modelBuilder.Entity<Symptom>().HasData(symptoms);

        // Seed symptom categories (reference table)
        var categories = new[]
        {
            new SymptomCategory { Id = 1, Name = "MildModerateSevere", DisplayValue = "Mild/Moderate/Severe" },
            new SymptomCategory { Id = 2, Name = "YesNo", DisplayValue = "Yes/No" },
            new SymptomCategory { Id = 3, Name = "Scale1To10", DisplayValue = "1-10 Scale" },
        };

        modelBuilder.Entity<SymptomCategory>().HasData(categories);

        // Seed symptom ranges (reference table)
        var symptomRanges = new List<SymptomRange>();

        // MildModerateSevere symptoms (category 1)
        var mildModerateSevereSymptoms = new[] { 1, 2, 4, 5, 6, 7, 8, 9, 11, 13, 14 };
        var mildModerateSevereValues = new[] { "Mild", "Moderate", "Severe" };
        int rangeId = 1;

        foreach (var symptomId in mildModerateSevereSymptoms)
        {
            foreach (var value in mildModerateSevereValues)
            {
                symptomRanges.Add(new SymptomRange { Id = rangeId++, SymptomId = symptomId, CategoryId = 1, SymptomValue = value });
            }
        }

        // YesNo symptoms (category 2)
        var yesNoSymptoms = new[] { 3, 10, 15, 16, 17, 18 };
        var yesNoValues = new[] { "No", "Yes" };

        foreach (var symptomId in yesNoSymptoms)
        {
            foreach (var value in yesNoValues)
            {
                symptomRanges.Add(new SymptomRange { Id = rangeId++, SymptomId = symptomId, CategoryId = 2, SymptomValue = value });
            }
        }

        // Scale1To10 symptoms (category 3)
        var scale1To10Symptoms = new[] { 12, 19 };
        var scale1To10Values = new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

        foreach (var symptomId in scale1To10Symptoms)
        {
            foreach (var value in scale1To10Values)
            {
                symptomRanges.Add(new SymptomRange { Id = rangeId++, SymptomId = symptomId, CategoryId = 3, SymptomValue = value });
            }
        }

        modelBuilder.Entity<SymptomRange>().HasData(symptomRanges);
    }
}