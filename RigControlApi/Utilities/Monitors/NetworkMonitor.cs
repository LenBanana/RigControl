using LibreHardwareMonitor.Hardware;
using Microsoft.AspNetCore.SignalR;
using RigControlApi.Hubs;
using RigControlApi.Hubs.Interfaces;
using RigControlApi.Models;
using RigControlApi.Utilities.Monitors.Base;

namespace RigControlApi.Utilities.Monitors;

public class NetworkMonitor : HardwareMonitor
{
    public NetworkMonitor(IServiceScopeFactory serviceScopeFactory)
        : base(serviceScopeFactory, new Computer() { IsNetworkEnabled = true }, "Network", new List<MonitorSensor>()
        {
            new(SensorType.Throughput, "Download Speed", "Download"),
            new(SensorType.Throughput, "Upload Speed", "Upload")
        })
    {
    }

    protected override void UpdateHub(IHubContext<HardwareHub, IHardwareHub> hubContext, MonitorDto monitorData)
    {
        hubContext?.Clients.All.SendNetworkSpeed(monitorData);
    }
}