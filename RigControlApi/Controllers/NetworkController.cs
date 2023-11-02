using Microsoft.AspNetCore.Mvc;
using RigControlApi.Utilities.Network;

namespace RigControlApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class NetworkController : Controller
{
    private readonly IServiceScopeFactory _serviceScope;
    public NetworkController(IServiceScopeFactory serviceScope)
    {
        _serviceScope = serviceScope;
    }
    
    [HttpPost]
    public IActionResult SetupNetworkMonitoring(string adapterName, int interval = 1000)
    {
        _ = new NetworkMonitor(adapterName, interval, _serviceScope);
        return Ok();
    }
    
    [HttpPost]
    public IActionResult StopNetworkMonitoring(string adapterName)
    {
        var monitor = NetworkMonitor.GetNetworkMonitor(adapterName);
        if (monitor == null) return BadRequest();
        monitor.StopMonitoring();
        monitor.Dispose();
        return Ok();
    }
}