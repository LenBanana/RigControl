using Hardware.Info;

namespace RigControlApi.Models;

public class HardwareDto
{
    public OS? OperatingSystem { get; set; }
    
    public List<Memory>? MemoryList { get; set; }
    
    public List<Drive>? DriveList { get; set; }
    
    public List<CPU>? CpuList { get; set; }
    
    public List<BIOS>? BiosList { get; set; }
    
    public List<VideoController>? VideoControllerList { get; set; }
    
    public List<NetworkDto>? NetworkAdapterList { get; set; }
}