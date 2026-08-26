using Microsoft.EntityFrameworkCore;
using RegSystemAPI.Data;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    // ปิดการโหลดไฟล์ซ้ำอัตโนมัติ เพื่อแก้ปัญหา inotify limit บน Linux Cloud
    WebRootPath = "wwwroot"
});

// Add services to the container.
// ผูก Oracle DbContext เข้ากับโปรเจกต์
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleDb")));

builder.Services.AddControllers();

// 📌 1. เพิ่ม CORS Policy ไว้ตรงนี้ (ก่อน builder.Build())
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 📌 2. เปิดใช้งาน CORS (ต้องวางไว้ก่อน app.MapControllers() เสมอ!)
app.UseCors("AllowAll");

// เปิดใช้งาน Controller Routes (เพื่อให้ API ที่เรากำลังจะเขียนทำงานได้)
app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}