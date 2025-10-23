using AH.CancerConnect.AdminAPI.Configurations;

var builder = WebApplication.CreateBuilder(args);

// 1) Logging (Serilog)
builder.AddSerilogLogging();

// 2) Core & App Services (controllers, options, DI registrations)
builder.Services.AddApplicationServices(builder.Configuration);

// 3) Database (DbContext)
builder.Services.AddApplicationDatabase(builder.Configuration);

// 4) API Versioning + Swagger
builder.Services.AddApiVersioningAndSwagger();

// 5) CORS - Allow all origins, methods, and headers
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Database migrations (safe to call in all envs)
await app.ApplyDatabaseMigrationsAsync();

if (!app.Environment.IsProduction())
{
    app.UseVersionedSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseCors();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
