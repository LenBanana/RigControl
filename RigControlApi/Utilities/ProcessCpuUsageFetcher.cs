using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading;
using RigControlApi.Interfaces;
using RigControlApi.Models;

namespace RigControlApi.Utilities;

public class ProcessCpuUsageFetcher : IHostedService, IProcessCpuUsageFetcher
{
    private static readonly BlockingCollection<UpdatingProcess> UpdatingProcesses = new();
    private static readonly HashSet<int> BlockedProcessIds = new();
    private Timer? _updateTimer;
    private CancellationToken _cancellationToken;

    public ProcessDto? GetProcess(int processId)
    {
        var updatedProcess = UpdatingProcesses.FirstOrDefault(p => p.LocalProcess.Id == processId);
        if (updatedProcess == null) return null;
        Process? process = null;
        try
        {
            process = Process.GetProcessById(processId);
        }
        catch
        {
            // ignored
        }

        if (process != null)
        {
            process.Dispose();
            return updatedProcess.LocalProcess;
        }

        if (UpdatingProcesses.TryTake(out var processToRemove))
        {
            processToRemove.Dispose();
        }

        return null;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        _updateTimer = new Timer(UpdateProcessesCallback, null, 0, 1000);
        return Task.CompletedTask;
    }

    private void UpdateProcessesCallback(object? state)
    {
        AddProcessesToUpdate(_cancellationToken);
    }

    private static void AddProcessesToUpdate(CancellationToken cancellationToken)
    {
        var processList = Process.GetProcesses();
        foreach (var process in processList)
        {
            try
            {
                if (BlockedProcessIds.Contains(process.Id)) continue;
                if (UpdatingProcesses.Any(p => p.LocalProcess.Id == process.Id)) continue;
            }
            catch
            {
                continue;
            }

            var processDto = new ProcessDto()
            {
                Id = process.Id,
                Name = process.ProcessName,
                MemoryUsage = process.WorkingSet64,
                ProcessorUsage = -1,
                StartTime = DateTime.MinValue,
                TotalProcessorTime = TimeSpan.Zero
            };
            try
            {
                processDto.TotalProcessorTime = process.TotalProcessorTime;
                processDto.StartTime = process.StartTime.ToUniversalTime();
                UpdatingProcesses.Add(new UpdatingProcess(processDto), cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding process to update: {ex.Message}");
                BlockedProcessIds.Add(process.Id);
            }
            finally
            {
                process.Dispose();
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _updateTimer?.Dispose();
        UpdatingProcesses.ToList().ForEach(p => p.Dispose());
        return Task.CompletedTask;
    }
}