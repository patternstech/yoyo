using AH.CancerConnect.API;
using AH.CancerConnect.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

// 1) Logging (Serilog)
builder.AddSerilogLogging();

// 2) Core & App Services (controllers, options, DI registrations)
builder.Services.AddApplicationServices(builder.Configuration);

// 3) Database (DbContext)
builder.Services.AddApplicationDatabase(builder.Configuration);

// 4) API Versioning + Swagger
builder.Services.AddApiVersioningAndSwagger();

var app = builder.Build();

// Database migrations (safe to call in all envs)
await app.ApplyDatabaseMigrationsAsync();

if (!app.Environment.IsProduction())
{
    app.UseVersionedSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();