using Microsoft.EntityFrameworkCore;

namespace AH.CancerConnect.AdminAPI.Configurations;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddApplicationDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CancerConnectDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("CancerConnectDB");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Missing connection string: CancerConnectDB");
            }

            // Replace |DataDirectory| with the app base directory to support local files
            connectionString = connectionString.Replace("|DataDirectory|", AppContext.BaseDirectory);

            options.UseSqlServer(connectionString);
        });

        return services;
    }
}