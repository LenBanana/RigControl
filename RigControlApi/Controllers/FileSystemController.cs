using Microsoft.AspNetCore.Mvc;
using RigControlApi.Utilities.FileSystem;

namespace RigControlApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class FileSystemController : Controller
{
    [HttpGet]
    public IActionResult GetBaseItems()
    {
        return Ok(FileSystemNavigation.GetBaseItems());
    }
    
    [HttpGet]
    public IActionResult GetItems(string path, string searchPattern = "*")
    {
        return Ok(FileSystemNavigation.GetItems(path, searchPattern));
    }
    
    [HttpGet]
    public IActionResult GetFolders(string path)
    {
        return Ok(FileSystemNavigation.GetFolders(path));
    }
    
    [HttpGet]
    public IActionResult GetFiles(string path, string searchPattern = "*")
    {
        return Ok(FileSystemNavigation.GetFiles(path, searchPattern));
    }
}