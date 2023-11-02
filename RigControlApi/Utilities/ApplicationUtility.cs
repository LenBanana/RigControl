using System.Diagnostics;
using RigControlApi.Interfaces;
using RigControlApi.Models;

namespace RigControlApi.Utilities;

public class ApplicationUtility
{
    private readonly IProcessCpuUsageFetcher _processCpuUsageFetcher;
    public ApplicationUtility(IProcessCpuUsageFetcher processCpuUsageFetcher)
    {
        _processCpuUsageFetcher = processCpuUsageFetcher;
    }
    public List<ProcessDto?> GetRunningApplications()
    {
        var processList = Process.GetProcesses();
        return processList.Select(process => _processCpuUsageFetcher.GetProcess(process.Id)).Where(p => p != null && (p.MemoryUsage > -1 || p.ProcessorUsage > 0)).OrderByDescending(p => p?.ProcessorUsage).ToList();
    }
}