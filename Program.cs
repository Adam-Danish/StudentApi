using Microsoft.EntityFrameworkCore;
using StudentApi.Models;

var builderOptions = new WebApplicationOptions { Args = args };
var builder = WebApplication.CreateBuilder(builderOptions);

// Prevent file watcher crash on Linux containers
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    foreach (var source in config.Sources)
    {
        if (source is Microsoft.Extensions.Configuration.FileConfigurationSource fileSource)
        {
            fileSource.ReloadOnChange = false;
        }
    }
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=student.db"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student API v1");
    c.RoutePrefix = string.Empty; // Serves Swagger UI at application root
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.MapControllers();
app.Run();