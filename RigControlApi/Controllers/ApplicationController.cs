using Microsoft.AspNetCore.Mvc;
using RigControlApi.Utilities;
using RigControlApi.Utilities.PcControl;

namespace RigControlApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ApplicationController : Controller
{
    private readonly ApplicationUtility _applicationUtility;

    public ApplicationController(ApplicationUtility applicationUtility)
    {
        _applicationUtility = applicationUtility;
    }

    [HttpGet]
    public IActionResult GetRunningApplications()
    {
        var runningApplications = _applicationUtility.GetRunningApplications();
        return Ok(runningApplications);
    }

    [HttpPost]
    public IActionResult CloseApplication(int applicationId)
    {
        ApplicationCommands.CloseApplication(applicationId);
        return Ok();
    }
}