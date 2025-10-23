using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace AH.CancerConnect.AdminAPI.Configurations;

public static class SwaggerVersioningConfiguration
{
    public static IServiceCollection AddApiVersioningAndSwagger(this IServiceCollection services)
    {
        // API Versioning
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new HeaderApiVersionReader("api-Version"));
        })
        .AddMvc()
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
            options.DefaultApiVersion = new ApiVersion(1);
            options.AssumeDefaultVersionWhenUnspecified = true;
        });

        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            var asm = typeof(Program).Assembly;
            var xmlFile = $"{asm.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }

            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API V1", Version = "v1", Description = "API v1" });
            c.SwaggerDoc("v2", new OpenApiInfo { Title = "API V2", Version = "v2", Description = "API v2" });
        });

        return services;
    }

    public static IApplicationBuilder UseVersionedSwaggerUI(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            var descriptions = app.ApplicationServices
                .GetRequiredService<IApiVersionDescriptionProvider>()
                .ApiVersionDescriptions;

            foreach (var desc in descriptions)
            {
                var url = $"/swagger/{desc.GroupName}/swagger.json";
                var name = desc.GroupName.ToUpperInvariant();
                c.SwaggerEndpoint(url, name);
            }
        });

        return app;
    }
}
