using System.Collections.Concurrent;
using Timer = System.Timers.Timer;

namespace RigControlApi.Interfaces;

public interface ISystemMonitor
{
    private static readonly ConcurrentDictionary<string, ISystemMonitor> NetworkMonitors = new();
    Timer? MonitorTimer { get; set; }
    string Id { get; set; }
    void StartMonitoring();

    void StopMonitoring();
}