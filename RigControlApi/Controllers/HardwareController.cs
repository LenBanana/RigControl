using Hardware.Info;
using LibreHardwareMonitor.Hardware;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RigControlApi.Utilities;

namespace RigControlApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class HardwareController : Controller
{
    private readonly HardwareUtility _hardwareUtility;

    private readonly IServiceScopeFactory _serviceScope;
    public HardwareController(HardwareUtility hardwareUtility,IServiceScopeFactory serviceScope)
    {
        _hardwareUtility = hardwareUtility;
        _serviceScope = serviceScope;
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

    [HttpGet]
    public IActionResult GetAllSensorInfo()
    {
        var computer = new Computer()
        {
            IsControllerEnabled = true,
            IsMotherboardEnabled = true,
            IsPsuEnabled = true
        };
        computer.Open();
        foreach (var hardware in computer.Hardware)
        {
            hardware.Update();
            Console.WriteLine("Hardware: {0}", hardware.Name);

            foreach (var sensor in hardware.Sensors)
            {
                Console.WriteLine("\tSensor: {0}, value: {1}, type: {2}", sensor.Name, sensor.Value, sensor.SensorType);
                foreach (var subHw in hardware.SubHardware)
                {
                    subHw.Update();
                    Console.WriteLine("SubHardware: {0}", subHw.Name);
                    foreach (var subSensor in subHw.Sensors)
                    {
                        Console.WriteLine("\tSensor: {0}, value: {1}, type: {2}", subSensor.Name, subSensor.Value, subSensor.SensorType);
                    }
                }
            }
        }

        return Ok();
    }
}