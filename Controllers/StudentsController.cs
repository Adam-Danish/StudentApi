using Microsoft.EntityFrameworkCore;
using StudentApi.Models; // Change to your actual Models namespace if different

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure SQLite DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=student.db"));

var app = builder.Build();

// --- Enable Swagger UI in ALL environments (including Production / Render) ---
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student API v1");
    c.RoutePrefix = string.Empty; // Serves Swagger directly at the root URL (https://studentapi-hc5t.onrender.com/)
});

// Auto-create database & tables on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// CRITICAL: Registers controller routes like /api/students
app.MapControllers();

app.Run();