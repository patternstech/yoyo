using Microsoft.EntityFrameworkCore;
using Serilog;

namespace AH.CancerConnect.AdminAPI.Configurations;

public static class DatabaseMigrations
{
    public static async Task ApplyDatabaseMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<CancerConnectDbContext>();

            Log.Debug("Starting database migration process...");

            // Check if database exists
            var canConnect = await dbContext.Database.CanConnectAsync();

            if (!canConnect)
            {
                Log.Debug("Database does not exist. Creating database using migrations...");
                await dbContext.Database.MigrateAsync();
                Log.Debug("Database created and migrations applied successfully.");
            }
            else
            {
                // Database exists, check migration status
                var appliedMigrations = await dbContext.Database.GetAppliedMigrationsAsync();
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

                Log.Debug(
                    "Database exists. Applied migrations: {AppliedCount}, Pending: {PendingCount}",
                    appliedMigrations.Count(),
                    pendingMigrations.Count());

                if (pendingMigrations.Any())
                {
                    Log.Debug(
                        "Applying {Count} pending migrations: {Migrations}",
                        pendingMigrations.Count(),
                        string.Join(", ", pendingMigrations));

                    await dbContext.Database.MigrateAsync();
                    Log.Debug("Database migrations completed successfully.");
                }
                else
                {
                    Log.Debug("No pending migrations found. Database is up to date.");
                }
            }

            // Verify the database has data
            var providersCount = await dbContext.Providers.CountAsync();
            Log.Debug("Database verification: {ProvidersCount} providers found", providersCount);

            if (providersCount == 0)
            {
                Log.Warning("No symptoms found in database. Check if seed data was applied correctly.");
            }
            else
            {
                Log.Debug("Database is healthy with {SymptomCount} symptoms available", providersCount);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to initialize or migrate the database.");

            if (ex.Message.Contains("There is already an object named"))
            {
                Log.Warning("Database schema conflict detected.");
                Log.Debug("To resolve this issue, run: dotnet ef database drop --force && dotnet ef database update");
            }

            // Don't crash the app, but log the error
            Log.Warning("Application will continue, but database functionality may be impaired.");
        }
    }
}