using RigControlApi.Hubs;
using RigControlApi.Hubs.Interfaces;
using RigControlApi.Interfaces;
using RigControlApi.Utilities;

var builder = WebApplication.CreateBuilder(args);
var allowedOrigin = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Cors", policy =>
    {
        if (allowedOrigin != null)
            policy.WithOrigins(allowedOrigin)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        ;
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSingleton<IProcessCpuUsageFetcher, ProcessCpuUsageFetcher>(); 
builder.Services.AddHostedService<ProcessCpuUsageFetcher>();
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

app.MapHub<NetworkDataHub>("/networkDataHub");

app.Run();