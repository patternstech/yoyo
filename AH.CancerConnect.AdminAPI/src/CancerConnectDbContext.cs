using AH.CancerConnect.AdminAPI.Features.Provider;
using AH.CancerConnect.AdminAPI.Features.SymptomConfiguration;
using Microsoft.EntityFrameworkCore;
 
namespace AH.CancerConnect.AdminAPI;
 
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
    /// Gets or sets the providers table.
    /// </summary>
    public DbSet<Provider> Providers { get; set; }
 
    /// <summary>
    /// Gets or sets the provider pools table.
    /// </summary>
    public DbSet<ProviderPool> ProviderPools { get; set; }

    /// <summary>
    /// Gets or sets the symptoms table (reference).
    /// </summary>
    public DbSet<Symptom> Symptoms { get; set; }

    /// <summary>
    /// Gets or sets the symptom configurations table.
    /// </summary>
    public DbSet<SymptomConfiguration> SymptomConfigurations { get; set; }
 
    // Add these table configurations in your OnModelCreating method
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
 
        // Configure table names
        modelBuilder.Entity<Provider>().ToTable("Provider", "dbo");
        modelBuilder.Entity<ProviderPool>().ToTable("ProviderPool", "dbo");
        modelBuilder.Entity<Symptom>().ToTable("Symptom", "ref");
        modelBuilder.Entity<SymptomConfiguration>().ToTable("SymptomConfiguration", "dbo");
 
        // Configure ProviderPool entity
        modelBuilder.Entity<ProviderPool>(entity =>
        {
            entity.HasKey(pp => pp.Id);
            entity.Property(pp => pp.Name).IsRequired().HasMaxLength(200);
            entity.Property(pp => pp.Description).HasMaxLength(500);
            entity.Property(pp => pp.IsActive).IsRequired().HasDefaultValue(true);
            entity.Property(pp => pp.DateCreated).IsRequired();
            entity.Property(pp => pp.DateModified).IsRequired();
 
            // Index for performance
            entity.HasIndex(pp => pp.IsActive);
            entity.HasIndex(pp => pp.Name);
        });
 
        // Configure Provider entity
        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(p => p.LastName).IsRequired().HasMaxLength(100);
            entity.Property(p => p.ProviderId).IsRequired().HasMaxLength(50);
            entity.Property(p => p.ProviderPoolId);
            entity.Property(p => p.IsActive).IsRequired().HasDefaultValue(true);
            entity.Property(p => p.DateCreated).IsRequired();
            entity.Property(p => p.DateModified).IsRequired();
 
            // Unique constraint on ProviderId
            entity.HasIndex(p => p.ProviderId).IsUnique();
 
            // Relationship with ProviderPool
            entity.HasOne(p => p.ProviderPool)
                  .WithMany(pp => pp.Providers)
                  .HasForeignKey(p => p.ProviderPoolId)
                  .OnDelete(DeleteBehavior.SetNull);
 
            // Indexes for performance
            entity.HasIndex(p => p.IsActive);
            entity.HasIndex(p => new { p.LastName, p.FirstName });
            entity.HasIndex(p => p.ProviderPoolId);
        });

        // Configure Symptom entity (reference table - read-only from API project)
        modelBuilder.Entity<Symptom>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(250);
            entity.Property(s => s.DisplayTitle).IsRequired().HasMaxLength(250);
            entity.Property(s => s.Description).IsRequired().HasMaxLength(500);
            entity.Property(s => s.Invalid).IsRequired();
            
            // This table is managed by the API project, AdminAPI only reads from it
            entity.ToTable("Symptom", "ref", t => t.ExcludeFromMigrations());
        });

        // Configure SymptomConfiguration entity
        modelBuilder.Entity<SymptomConfiguration>(entity =>
        {
            entity.HasKey(sc => sc.Id);
            entity.Property(sc => sc.SymptomId).IsRequired();
            entity.Property(sc => sc.AlertTrigger).IsRequired().HasMaxLength(500);
            entity.Property(sc => sc.FollowUp).IsRequired();
            entity.Property(sc => sc.Question).HasMaxLength(1000);
            entity.Property(sc => sc.Created).IsRequired().HasMaxLength(250);
            entity.Property(sc => sc.IsActive).IsRequired().HasDefaultValue(true);
            entity.Property(sc => sc.DateCreated).IsRequired();
            entity.Property(sc => sc.DateModified).IsRequired();

            // Relationship with Symptom
            entity.HasOne(sc => sc.Symptom)
                  .WithMany(s => s.SymptomConfigurations)
                  .HasForeignKey(sc => sc.SymptomId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Indexes for performance
            entity.HasIndex(sc => sc.SymptomId);
            entity.HasIndex(sc => sc.IsActive);
        });
 
        // Seed provider pools
        var providerPools = new[]
        {
            new ProviderPool
            {
                Id = 1,
                Name = "Provider Pool A",
                Description = "Primary care team for oncology patients",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            new ProviderPool
            {
                Id = 2,
                Name = "Provider Pool B",
                Description = "Surgical care team for cancer treatment",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            new ProviderPool
            {
                Id = 3,
                Name = "Provider Pool C",
                Description = "Radiation and chemotherapy specialist team",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
        };
 
        modelBuilder.Entity<ProviderPool>().HasData(providerPools);

        // Note: Symptoms table is NOT seeded here - it's managed by the API project
        // AdminAPI only reads from the existing ref.Symptom table

        // Seed symptom configurations - based on the provided data file and grid
        var symptomConfigurations = new[]
        {
            // Anxiety - ID 1
            new SymptomConfiguration
            {
                Id = 1,
                SymptomId = 1,
                AlertTrigger = "When Severe is indicated for 1+ day or Moderate is indicated for 2+ days",
                FollowUp = false,
                Question = null,
                Created = "James Owen 2025-08-23",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            // Appetite Loss - ID 2
            new SymptomConfiguration
            {
                Id = 2,
                SymptomId = 2,
                AlertTrigger = "When 2 days of moderate or 1 score of Severe",
                FollowUp = true,
                Question = "Have you experienced moderate or severe appetite loss for more than two days?",
                Created = "James Owen 2025-08-23",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            // Bleeding - ID 3
            new SymptomConfiguration
            {
                Id = 3,
                SymptomId = 3,
                AlertTrigger = "When Yes is indicated for 1+ day",
                FollowUp = false,
                Question = "N/A",
                Created = "James Owen 2025-08-23",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            // Constipation - ID 5
            new SymptomConfiguration
            {
                Id = 4,
                SymptomId = 5,
                AlertTrigger = "When 2 days of moderate or 1 score of Severe",
                FollowUp = true,
                Question = "Have you experienced moderate or severe constipation for two or more days?",
                Created = "James Owen 2025-08-23",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            // Cough - ID 6
            new SymptomConfiguration
            {
                Id = 5,
                SymptomId = 6,
                AlertTrigger = "When 2 days of moderate or 1 score of Severe",
                FollowUp = true,
                Question = "Have you experienced moderate or severe cough for two or more days?",
                Created = "James Owen 2025-08-23",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            // Depression - ID 7
            new SymptomConfiguration
            {
                Id = 6,
                SymptomId = 7,
                AlertTrigger = "When Severe is indicated for 1+ day",
                FollowUp = false,
                Question = null,
                Created = "James Owen 2025-08-23",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            // Diarrhea - ID 8
            new SymptomConfiguration
            {
                Id = 7,
                SymptomId = 8,
                AlertTrigger = "When 2 days of moderate or 1 score of Severe",
                FollowUp = false,
                Question = null,
                Created = "James Owen 2025-08-23",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            // Edema/Swelling - ID 17
            new SymptomConfiguration
            {
                Id = 8,
                SymptomId = 17,
                AlertTrigger = "Trigger 1 day of Yes",
                FollowUp = false,
                Question = null,
                Created = "James Owen 2025-08-23",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            // Fatigue - ID 9
            new SymptomConfiguration
            {
                Id = 9,
                SymptomId = 9,
                AlertTrigger = "When 2 days of moderate or 1 score of Severe",
                FollowUp = false,
                Question = null,
                Created = "James Owen 2025-08-23",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            // Fever - ID 10
            new SymptomConfiguration
            {
                Id = 10,
                SymptomId = 10,
                AlertTrigger = "Trigger when 1 day of Yes",
                FollowUp = false,
                Question = "N/A",
                Created = "James Owen 2025-08-23",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            // Impaired activity (ADLs) - ID 4
            new SymptomConfiguration
            {
                Id = 11,
                SymptomId = 4,
                AlertTrigger = "When 2 days of moderate or 1 score of Severe",
                FollowUp = false,
                Question = null,
                Created = "James Owen 2025-08-23",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            // Level of Emotional Distress - ID 19
            new SymptomConfiguration
            {
                Id = 12,
                SymptomId = 19,
                AlertTrigger = "Trigger when score 4 or higher for 4 days in a row or score 4 or higher for 4 days within 10 days",
                FollowUp = false,
                Question = null,
                Created = "James Owen 2025-08-23",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            // Mouth Sores - ID 18
            new SymptomConfiguration
            {
                Id = 13,
                SymptomId = 18,
                AlertTrigger = "Trigger 1 day of Yes",
                FollowUp = false,
                Question = null,
                Created = "James Owen 2025-08-23",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            // Nausea or Vomiting - ID 11
            new SymptomConfiguration
            {
                Id = 14,
                SymptomId = 11,
                AlertTrigger = "When 1 days of moderate or 1 score of Severe",
                FollowUp = false,
                Question = null,
                Created = "James Owen 2025-08-23",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            // Pain - ID 12
            new SymptomConfiguration
            {
                Id = 15,
                SymptomId = 12,
                AlertTrigger = "When an 4 or above is indicated for 1 time",
                FollowUp = false,
                Question = "N/A",
                Created = "James Owen 2025-08-23",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            // Redness of skin - ID 16
            new SymptomConfiguration
            {
                Id = 16,
                SymptomId = 16,
                AlertTrigger = "Trigger 1 day of Yes",
                FollowUp = false,
                Question = null,
                Created = "James Owen 2025-08-23",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            // Shortness of breath - ID 14
            new SymptomConfiguration
            {
                Id = 17,
                SymptomId = 14,
                AlertTrigger = "When 1 days of moderate or 1 day of Severe",
                FollowUp = true,
                Question = "Have you experienced moderate or severe shortness of breath for more than 24 hours?",
                Created = "James Owen 2025-08-23",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            // Skin rash - ID 15
            new SymptomConfiguration
            {
                Id = 18,
                SymptomId = 15,
                AlertTrigger = "Trigger 1 day of Yes",
                FollowUp = false,
                Question = null,
                Created = "James Owen 2025-08-23",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            // Tingling and Numbness in hands or feet - ID 13
            new SymptomConfiguration
            {
                Id = 19,
                SymptomId = 13,
                AlertTrigger = "When Severe is indicated for 2+ days",
                FollowUp = true,
                Question = "Have you experienced severe tingling and/or numbness in your hands and feet for two or more days?",
                Created = "James Owen 2025-08-23",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
        };

        modelBuilder.Entity<SymptomConfiguration>().HasData(symptomConfigurations);
    }
}