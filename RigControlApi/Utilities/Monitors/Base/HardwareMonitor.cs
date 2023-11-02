using System.Timers;
using LibreHardwareMonitor.Hardware;
using Microsoft.AspNetCore.SignalR;
using RigControlApi.Hubs;
using RigControlApi.Hubs.Interfaces;
using RigControlApi.Interfaces;
using RigControlApi.Models;
using Timer = System.Timers.Timer;

namespace RigControlApi.Utilities.Monitors.Base;

public abstract class HardwareMonitor : IDisposable, ISystemMonitor, IHostedService
{
    public Timer? MonitorTimer { get; set; }
    public string Id { get; set; }
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly Computer _computer;
    private readonly Dictionary<string, ISensor?> _sensors;

    protected HardwareMonitor(IServiceScopeFactory serviceScopeFactory, Computer computer, string id, List<MonitorSensor> sensors)
    {
        _serviceScopeFactory = serviceScopeFactory;
        Id = id;
        _computer = computer;
        _computer.Open();

        MonitorTimer = new Timer(1000)
        {
            AutoReset = true,
            Enabled = true
        };
        MonitorTimer.Elapsed += CheckUsage;
        _sensors = new Dictionary<string, ISensor?>();
        InitializeSensors(sensors);
    }

    private void InitializeSensors(List<MonitorSensor> sensorTypes)
    {
        IHardware? hardware = null;
        foreach (var sensorType in sensorTypes)
        { 
            foreach (var h in _computer.Hardware)
            {
                h?.Update();
            }
            hardware ??= _computer.Hardware.MaxBy(h => h.Sensors.Max(s => s.Value));
            var sensor = hardware?.Sensors.OrderByDescending(s => s.Max).FirstOrDefault(s =>
                s.SensorType == sensorType.SensorType && string.Equals(s.Name, sensorType.Name, StringComparison.CurrentCultureIgnoreCase));
            if (sensor != null) _sensors.Add(sensorType.Label, sensor);
        }
    }

    protected abstract void UpdateHub(IHubContext<HardwareHub, IHardwareHub> hubContext, MonitorDto monitorData);

    private void CheckUsage(object? sender, ElapsedEventArgs e)
    {
        try
        {
            var hubContext = _serviceScopeFactory.CreateScope().ServiceProvider
                .GetService<IHubContext<HardwareHub, IHardwareHub>>();
            var monitorData = GetUsage();
            if (hubContext != null) UpdateHub(hubContext, monitorData);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private MonitorDto GetUsage()
    {
        foreach (var hardware in _computer.Hardware)
        {
            hardware.Update();
        }
        return new MonitorDto(Id)
        {
            Data = _sensors.Select(sensor => new LabeledData(sensor.Key, sensor.Value?.Value ?? 0)).ToList()
        };
    }

    public void StartMonitoring()
    {
        MonitorTimer?.Start();
    }

    public void StopMonitoring()
    {
        MonitorTimer?.Stop();
    }

    public void Dispose()
    {
        MonitorTimer?.Dispose();
        _computer.Close();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        StartMonitoring();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        StopMonitoring();
        Dispose();
        return Task.CompletedTask;
    }
}
