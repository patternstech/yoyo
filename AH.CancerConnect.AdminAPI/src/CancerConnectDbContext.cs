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

    /// <summary>
    /// Configures the model and relationships for the database context.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure table names
        ConfigureTableNames(modelBuilder);

        // Configure entities
        ConfigureProviderPool(modelBuilder);
        ConfigureProvider(modelBuilder);
        ConfigureSymptom(modelBuilder);
        ConfigureSymptomConfiguration(modelBuilder);

        // Seed initial data
        SeedData(modelBuilder);
    }

    /// <summary>
    /// Configures table names for all entities.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureTableNames(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Provider>().ToTable("Provider", "dbo");
        modelBuilder.Entity<ProviderPool>().ToTable("ProviderPool", "dbo");
        modelBuilder.Entity<Symptom>().ToTable("Symptom", "ref");
        modelBuilder.Entity<SymptomConfiguration>().ToTable("SymptomConfiguration", "dbo");
    }

    /// <summary>
    /// Configures the ProviderPool entity.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureProviderPool(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProviderPool>(entity =>
        {
            entity.HasKey(pp => pp.Id);
            entity.Property(pp => pp.ProviderPoolId).IsRequired();
            entity.Property(pp => pp.Name).IsRequired().HasMaxLength(200);
            entity.Property(pp => pp.Description).HasMaxLength(500);
            entity.Property(pp => pp.CreatedBy).IsRequired().HasMaxLength(100);
            entity.Property(pp => pp.DateCreated).IsRequired();
            entity.Property(pp => pp.DateModified).IsRequired();

            // Index for performance
            entity.HasIndex(pp => pp.ProviderPoolId).IsUnique();
            entity.HasIndex(pp => pp.Name);
        });
    }

    /// <summary>
    /// Configures the Provider entity.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureProvider(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(p => p.LastName).IsRequired().HasMaxLength(100);
            entity.Property(p => p.ProviderId).IsRequired().HasMaxLength(50);
            entity.Property(p => p.Email).HasMaxLength(200);
            entity.Property(p => p.ProviderPoolId);
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
            entity.HasIndex(p => new { p.LastName, p.FirstName });
            entity.HasIndex(p => p.ProviderPoolId);
        });
    }

    /// <summary>
    /// Configures the Symptom entity (reference table - read-only from API project).
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureSymptom(ModelBuilder modelBuilder)
    {
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
    }

    /// <summary>
    /// Configures the SymptomConfiguration entity.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureSymptomConfiguration(ModelBuilder modelBuilder)
    {
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
    }

    /// <summary>
    /// Seeds initial data for the database.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Seed provider pools
        var providerPools = new[]
        {
            new ProviderPool
            {
                Id = 1,
                ProviderPoolId = 1,
                Name = "Provider Pool A",
                Description = "Primary care team for oncology patients",
                CreatedBy = "System",
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            new ProviderPool
            {
                Id = 2,
                ProviderPoolId = 2,
                Name = "Provider Pool B",
                Description = "Surgical care team for cancer treatment",
                CreatedBy = "System",
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
            new ProviderPool
            {
                Id = 3,
                ProviderPoolId = 3,
                Name = "Provider Pool C",
                Description = "Radiation and chemotherapy specialist team",
                CreatedBy = "System",
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
        };

        modelBuilder.Entity<ProviderPool>().HasData(providerPools);

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
                AlertTrigger = "Trigger 1 day of Yes",
                FollowUp = false,
                Question = null,
                Created = "James Owen 2025-08-23",
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            },
        };

        modelBuilder.Entity<SymptomConfiguration>().HasData(symptomConfigurations);
    }
}