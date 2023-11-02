using Hardware.Info;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RigControlApi.Utilities;

namespace RigControlApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class HardwareController : Controller
{
    private readonly HardwareUtility _hardwareUtility;

    public HardwareController(HardwareUtility hardwareUtility)
    {
        _hardwareUtility = hardwareUtility;
    }
    
    [HttpGet]
    public IActionResult GetOperatingSystemInfo(bool refresh = false)
    {
        return Ok(_hardwareUtility.GetOperatingSystem(refresh));
    }
    
    [HttpGet]
    public IActionResult GetDriveInfo(bool refresh = false)
    {
        return Ok(_hardwareUtility.GetDriveList(refresh));
    }
    
    [HttpGet]
    public IActionResult GetMemoryInfo(bool refresh = false)
    {
        return Ok(_hardwareUtility.GetMemoryList(refresh));
    }
    
    [HttpGet]
    public IActionResult GetBiosInfo(bool refresh = false)
    {
        return Ok(_hardwareUtility.GetBiosList(refresh));
    }
    
    [HttpGet]
    public IActionResult GetVideoControllerInfo(bool refresh = false)
    {
        return Ok(_hardwareUtility.GetVideoControllerList(refresh));
    }
    
    [HttpGet]
    public IActionResult GetCpuInfo(bool refresh = false)
    {
        return Ok(_hardwareUtility.GetCpuList(refresh));
    }

    [HttpGet]
    public IActionResult GetNetworkInfo(bool refresh = false)
    {
        return Ok(_hardwareUtility.GetNetworkAdapterList(refresh));
    }

    [HttpPost]
    public IActionResult CpuMonitoring(string processorId)
    {
        return Ok();
    }
}