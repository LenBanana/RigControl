using RigControlApi.Models;

namespace RigControlApi.Interfaces;

public interface IProcessCpuUsageFetcher
{
    ProcessDto? GetProcess(int processId);
}