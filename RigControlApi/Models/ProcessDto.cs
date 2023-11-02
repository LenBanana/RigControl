namespace RigControlApi.Models;

public class ProcessDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double MemoryUsage { get; set; }
    public double ProcessorUsage { get; set; }
    public TimeSpan TotalProcessorTime { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public bool IsElevated { get; set; } = false;
}