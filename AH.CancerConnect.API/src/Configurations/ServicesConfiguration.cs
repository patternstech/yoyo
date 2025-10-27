using AH.CancerConnect.API.Features.AuthToken;
using AH.CancerConnect.API.Features.Drainage.DrainageSetup;
using AH.CancerConnect.API.Features.Notes;
using AH.CancerConnect.API.Features.SymptomsTracking;
using AH.CancerConnect.API.Features.ToDo;
using AH.CancerConnect.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AH.CancerConnect.API.Configurations;

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
            
            // Add validation filter for HTTP 422 responses
            options.Filters.Add<ValidationFilter>();
        })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

        // Bind options (AuthSettings, XealthAppSettings)
        services.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));
        services.Configure<XealthAppSettings>(configuration.GetSection("XealthAppSettings"));

        // Core app services (DI)
        services.AddScoped<ITokenService, XealthTokenService>();
        services.AddScoped<IOrderRetrieveService, OrderRetrieveService>();
        services.AddScoped<IOrderEventNotificationService, OrderEventNotificationService>();
        services.AddScoped<ISymptomDataService, SymptomDataService>();
        services.AddScoped<INoteDataService, NoteDataService>();
        services.AddScoped<IToDoDataService, ToDoDataService>();
        services.AddScoped<IDrainageSetupDataService, DrainageSetupDataService>();

        // HttpClient factory
        services.AddHttpClient();

        // Optional: if you ever need consistent 400 details, etc.
        services.Configure<ApiBehaviorOptions>(o =>
        {
            // Customize model state errors here if desired
        });

        return services;
    }
}