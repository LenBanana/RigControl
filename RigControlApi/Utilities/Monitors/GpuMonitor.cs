using LibreHardwareMonitor.Hardware;
using Microsoft.AspNetCore.SignalR;
using RigControlApi.Hubs;
using RigControlApi.Hubs.Interfaces;
using RigControlApi.Models;
using RigControlApi.Utilities.Monitors.Base;

namespace RigControlApi.Utilities.Monitors;

public class GpuMonitor : HardwareMonitor
{
    public GpuMonitor(IServiceScopeFactory serviceScopeFactory)
        : base(serviceScopeFactory, new Computer() { IsGpuEnabled = true }, "GPU", new List<MonitorSensor>()
        {
            new(SensorType.Temperature, "GPU Core", "GPU Temperature"),
            new(SensorType.Load, "GPU Core", "GPU Usage")
        })
    {
    }

    protected override void UpdateHub(IHubContext<HardwareHub, IHardwareHub> hubContext, MonitorDto monitorData)
    {
        hubContext?.Clients.All.SendGpuMonitor(monitorData);
    }
}