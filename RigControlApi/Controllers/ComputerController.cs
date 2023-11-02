using Microsoft.AspNetCore.Mvc;
using RigControlApi.Utilities.PcControl;

namespace RigControlApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ComputerController : Controller
{

    [HttpPost]
    public IActionResult Shutdown()
    {
        ComputerCommands.Shutdown();
        return Ok();
    }

    [HttpPost]
    public IActionResult ShutdownAfter(int seconds)
    {
        ComputerCommands.Shutdown(seconds);
        return Ok();
    }

    [HttpPost]
    public IActionResult Restart()
    {
        ComputerCommands.Restart();
        return Ok();
    }

    [HttpPost]
    public IActionResult RestartAfter(int seconds)
    {
        ComputerCommands.Restart(seconds);
        return Ok();
    }

    [HttpPost]
    public IActionResult LogOff()
    {
        ComputerCommands.LogOff();
        return Ok();
    }

    [HttpPost]
    public IActionResult LogOffAfter(int seconds)
    {
        ComputerCommands.LogOff(seconds);
        return Ok();
    }

    [HttpPost]
    public IActionResult Hibernate()
    {
        ComputerCommands.Hibernate();
        return Ok();
    }

    [HttpPost]
    public IActionResult CancelShutdownCommand()
    {
        ComputerCommands.CancelShutdownCommand();
        return Ok();
    }
}