using System.Text.Json.Serialization;
using RigControlApi.Hubs;
using RigControlApi.Hubs.Interfaces;
using RigControlApi.Interfaces;
using RigControlApi.Utilities;
using RigControlApi.Utilities.Monitors;

var builder = WebApplication.CreateBuilder(args);
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Cors", policy =>
    {
        policy.SetIsOriginAllowedToAllowWildcardSubdomains()
            .SetIsOriginAllowed(_ => true)
            .WithOrigins(allowedOrigins ?? Array.Empty<string>())
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        ;
    });
});

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
});
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSingleton<IProcessCpuUsageFetcher, ProcessCpuUsageFetcher>();
builder.Services.AddHostedService<ProcessCpuUsageFetcher>();
// Add monitor services
builder.Services.AddHostedService<CpuMonitor>();
builder.Services.AddHostedService<GpuMonitor>();
builder.Services.AddHostedService<NetworkMonitor>();
builder.Services.AddHostedService<MemoryMonitor>();
// Add utility services
builder.Services.AddSingleton<HardwareUtility>();
builder.Services.AddSingleton<ApplicationUtility>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("Cors");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<HardwareHub>("/hardwareHub");

app.Run();