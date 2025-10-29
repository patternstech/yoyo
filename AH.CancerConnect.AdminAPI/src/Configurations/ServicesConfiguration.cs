using AH.CancerConnect.AdminAPI.Features.Provider;
using AH.CancerConnect.AdminAPI.Features.ProviderPool;
using AH.CancerConnect.AdminAPI.Features.SymptomConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace AH.CancerConnect.AdminAPI.Configurations;

public static class ServicesConfiguration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Controllers + JSON shape
        services.AddControllers(options =>
        {
            // Add global exception filter
            options.Filters.Add<GlobalExceptionFilter>();
        })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });


        // Core app services (DI)
        services.AddScoped<IProviderDataService, ProviderDataService>();
        services.AddScoped<IProviderPoolDataService, ProviderPoolDataService>();
        services.AddScoped<ISymptomConfigurationDataService, SymptomConfigurationDataService>();

        // HttpClient factory
        services.AddHttpClient();

 
        services.Configure<ApiBehaviorOptions>(o =>
        {
            
        });

        return services;
    }
}