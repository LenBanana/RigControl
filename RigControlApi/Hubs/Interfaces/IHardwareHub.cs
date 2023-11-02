using RigControlApi.Models;

namespace RigControlApi.Hubs.Interfaces;

public interface IHardwareHub
{
    public Task SendCpuMonitor(MonitorDto monitorDto);
    public Task SendGpuMonitor(MonitorDto monitorDto);
    public Task SendNetworkSpeed(MonitorDto networkSpeed);
    
    public Task SendMemoryMonitor(MonitorDto monitorDto);
}