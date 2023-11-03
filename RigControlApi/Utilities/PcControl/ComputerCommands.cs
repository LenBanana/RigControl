using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RigControlApi.Utilities.PcControl;

public abstract class ComputerCommands
{
    private static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    public static void Shutdown()
    {
        ExecuteCommand("shutdown", IsWindows ? "/s /t 0" : "-h now");
    }

    public static void Shutdown(int seconds)
    {
        ExecuteCommand("shutdown", IsWindows ? $"/s /t {seconds}" : $"-h +{seconds / 60}");
    }

    public static void Restart()
    {
        ExecuteCommand("shutdown", IsWindows ? "/r /t 0" : "-r now");
    }

    public static void Restart(int seconds)
    {
        ExecuteCommand("shutdown", IsWindows ? $"/r /t {seconds}" : $"-r +{seconds / 60}");
    }

    public static void LogOff()
    {
        if (IsWindows)
        {
            ExecuteCommand("shutdown", "/l /t 0");
        }
        else
        {
            ExecuteCommand("pkill", "-KILL -u " + Environment.UserName);
        }
    }

    public static void LogOff(int seconds)
    {
        if (IsWindows)
        {
            ExecuteCommand("shutdown", $"/l /t {seconds}");
        }
        else
        {
            // No direct equivalent on Linux, consider using 'at' command or similar
        }
    }

    public static void Hibernate()
    {
        if (IsWindows)
        {
            ExecuteCommand("shutdown", "/h");
        }
        else
        {
            ExecuteCommand("systemctl", "hibernate");
        }
    }

    public static void CancelShutdownCommand()
    {
        ExecuteCommand("shutdown", IsWindows ? "/a" : "-c");
    }

    private static void ExecuteCommand(string command, string arguments)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        process.Start();
    }
}