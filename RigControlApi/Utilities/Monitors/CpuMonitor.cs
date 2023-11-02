using LibreHardwareMonitor.Hardware;
using Microsoft.AspNetCore.SignalR;
using RigControlApi.Hubs;
using RigControlApi.Hubs.Interfaces;
using RigControlApi.Models;
using RigControlApi.Utilities.Monitors.Base;

namespace RigControlApi.Utilities.Monitors;

public class CpuMonitor : HardwareMonitor
{
    public CpuMonitor(IServiceScopeFactory serviceScopeFactory)
        : base(serviceScopeFactory, new Computer() { IsCpuEnabled = true }, "CPU", new List<MonitorSensor>()
        {
            new(SensorType.Temperature, "Core (Tctl/Tdie)", "CPU Temperature"),
            new(SensorType.Load, "CPU Total", "CPU Usage")
        })
    {
    }

    protected override void UpdateHub(IHubContext<HardwareHub, IHardwareHub> hubContext, MonitorDto monitorData)
    {
        hubContext?.Clients.All.SendCpuMonitor(monitorData);
    }
}