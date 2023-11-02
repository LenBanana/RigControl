using System.Diagnostics;
using System.Timers;
using RigControlApi.Models;
using Timer = System.Threading.Timer;

namespace RigControlApi.Utilities;

public class UpdatingProcess : IDisposable
{
    public ProcessDto LocalProcess { get; private set; }
    private DateTime _lastCaptureTime;
    private readonly Process? _process;
    private readonly Timer? _updateTimer;

    public UpdatingProcess(ProcessDto localProcess)
    {
        LocalProcess = localProcess;
        _lastCaptureTime = DateTime.UtcNow;
        try
        {
            _process = Process.GetProcessById(LocalProcess.Id);
            _updateTimer = new Timer(UpdateProcessCallback, null, 0, Timeout.Infinite);
            _process.Exited += ProcessOnExited;
        }
        catch
        {
            // ignored
        }
    }

    private void ProcessOnExited(object? sender, EventArgs e)
    {
        LocalProcess.ProcessorUsage = double.MinValue;
        LocalProcess.MemoryUsage = double.MinValue;
        LocalProcess.IsElevated = true;
        Dispose();
    }

    private void UpdateProcessCallback(object? state)
    {
        UpdateProcess();
    }

    private void UpdateProcess()
    {
        if (_process == null)
        {
            LocalProcess.ProcessorUsage = double.MinValue;
            Dispose();
            return;
        }

        LocalProcess.LastUpdated = DateTime.UtcNow;
        if (LocalProcess.IsElevated) 
        {
            ResetTimer();
            return;
        }
        try
        {
            LocalProcess.MemoryUsage = MemoryInformation.GetMemoryUsageForProcess(LocalProcess.Id);
            LocalProcess.ProcessorUsage = GetProcessCpuUsage();
        }
        catch (Exception ex)
        {
            LocalProcess.IsElevated = true;
            Console.WriteLine($"Error updating process: {ex.Message}");
        }
        ResetTimer();
    }

    private void ResetTimer()
    {
        _updateTimer?.Change(1000, Timeout.Infinite);
    }

    private double GetProcessCpuUsage()
    {
        if (_process == null)
        {
            return -1;
        }

        try
        {
            var currentTime = DateTime.UtcNow;
            var initialTime = LocalProcess.TotalProcessorTime;
            var elapsedRealTime = (currentTime - _lastCaptureTime).TotalSeconds;
            _lastCaptureTime = currentTime;  // Update the last capture time

            if (elapsedRealTime <= 0)  // Prevent division by zero
            {
                return LocalProcess.ProcessorUsage;  // Return the previous value
            }

            var timeDifference = _process.TotalProcessorTime - initialTime;
            LocalProcess.TotalProcessorTime = _process.TotalProcessorTime;  // Update the initial time

            var usagePercentage = (timeDifference.TotalSeconds / elapsedRealTime) * 100;
            return usagePercentage;
        }
        catch
        {
            return -1;
        }
    }

    public void Dispose()
    {
        _updateTimer?.Dispose();
        _process?.Dispose();
    }
}