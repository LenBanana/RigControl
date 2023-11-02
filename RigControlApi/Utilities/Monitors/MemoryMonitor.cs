using LibreHardwareMonitor.Hardware;
using Microsoft.AspNetCore.SignalR;
using RigControlApi.Hubs;
using RigControlApi.Hubs.Interfaces;
using RigControlApi.Models;
using RigControlApi.Utilities.Monitors.Base;

namespace RigControlApi.Utilities.Monitors;

public class MemoryMonitor : HardwareMonitor
{
    public MemoryMonitor(IServiceScopeFactory serviceScopeFactory)
        : base(serviceScopeFactory, new Computer() { IsMemoryEnabled = true }, "Memory", new List<MonitorSensor>()
        {
            new(SensorType.Load, "Memory", "Memory Usage"),
            new(SensorType.Load, "Virtual Memory", "Virtual Usage")
        })
    {
    }

    protected override void UpdateHub(IHubContext<HardwareHub, IHardwareHub> hubContext, MonitorDto monitorData)
    {
        hubContext?.Clients.All.SendMemoryMonitor(monitorData);
    }
}