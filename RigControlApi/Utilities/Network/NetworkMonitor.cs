using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Timers;
using Microsoft.AspNetCore.SignalR;
using RigControlApi.Hubs;
using RigControlApi.Hubs.Interfaces;
using RigControlApi.Models;

namespace RigControlApi.Utilities.Network;

public class NetworkMonitor : IDisposable
{
    private static readonly BlockingCollection<NetworkMonitor> NetworkMonitors = new();
    private readonly string _networkInterfaceId;
    private readonly PerformanceCounter _bytesSentCounter;
    private readonly PerformanceCounter _bytesReceivedCounter;
    private readonly System.Timers.Timer _monitorTimer;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public NetworkMonitor(string networkInterfaceId, double interval, IServiceScopeFactory serviceScopeFactory)
    {
        _networkInterfaceId = networkInterfaceId;
        if (string.IsNullOrWhiteSpace(networkInterfaceId))
        {
            throw new ArgumentException("Network interface name cannot be null or empty.");
        }
        if (NetworkMonitors.Any(nm => nm._networkInterfaceId.Equals(networkInterfaceId, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("Only one instance of this network monitor can be created.");
        }
        _serviceScopeFactory = serviceScopeFactory;
        // Add this network monitor to the collection
        NetworkMonitors.Add(this);
        // Find the network interface
        var selectedNetworkInterface = NetworkInterface.GetAllNetworkInterfaces()
            .FirstOrDefault(ni => ni.Name.Equals(networkInterfaceId, StringComparison.OrdinalIgnoreCase));

        if (selectedNetworkInterface == null)
        {
            throw new ArgumentException($"Network interface '{networkInterfaceId}' not found.");
        }

        // Set up performance counters using the selected network interface
        var instanceName = GetInstanceNameForNetworkInterface(selectedNetworkInterface);
        _bytesSentCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instanceName, readOnly: true);
        _bytesReceivedCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", instanceName, readOnly: true);

        // Set up the timer to check for network usage
        _monitorTimer = new System.Timers.Timer(interval)
        {
            AutoReset = true,
            Enabled = true
        };
        _monitorTimer.Elapsed += CheckNetworkUsage;
        StartMonitoring();
    }
    
    public static NetworkMonitor? GetNetworkMonitor(string networkInterfaceName)
    {
        return NetworkMonitors.FirstOrDefault(nm => nm._networkInterfaceId.Equals(networkInterfaceName, StringComparison.OrdinalIgnoreCase));
    }

    private string GetInstanceNameForNetworkInterface(NetworkInterface ni)
    {
        return ni.Description.Replace("#", "_").Replace("(", "[").Replace(")", "]");
    }

    // Method to start monitoring
    private void StartMonitoring()
    {
        _monitorTimer.Start();
    }

    // Method to stop monitoring
    public void StopMonitoring()
    {
        _monitorTimer.Stop();
    }

    private void CheckNetworkUsage(object sender, ElapsedEventArgs e)
    {
        var bytesSent = _bytesSentCounter.NextValue();
        var bytesReceived = _bytesReceivedCounter.NextValue();

        // Raise the event with the current data
        using var scope = _serviceScopeFactory.CreateScope();
        var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<NetworkDataHub, INetworkDataHub>>();
        hubContext.Clients.All.SendNetworkSpeed(new NetworkSpeed
        {
            ConnectionId = _networkInterfaceId,
            BytesSentPerSec = (ulong)bytesSent,
            BytesReceivedPerSec = (ulong)bytesReceived
        });
    }
    
    public void Dispose()
    {
        _monitorTimer?.Dispose();
        _bytesSentCounter?.Dispose();
        _bytesReceivedCounter?.Dispose();
        NetworkMonitors.TryTake(out _);
    }
}