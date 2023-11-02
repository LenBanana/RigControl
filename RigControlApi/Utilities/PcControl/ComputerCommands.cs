using System.Diagnostics;

namespace RigControlApi.Utilities.PcControl;

public abstract class ComputerCommands
{
    public static void Shutdown()
    {
        Process.Start("shutdown", "/s /t 0");
    }
    
    public static void Shutdown(int seconds)
    {
        Process.Start("shutdown", $"/s /t {seconds}");
    }

    public static void Restart()
    {
        Process.Start("shutdown", "/r /t 0");
    }
    
    public static void Restart(int seconds)
    {
        Process.Start("shutdown", $"/r /t {seconds}");
    }

    public static void LogOff()
    {
        Process.Start("shutdown", "/l /t 0");
    }
    
    public static void LogOff(int seconds)
    {
        Process.Start("shutdown", $"/l /t {seconds}");
    }

    public static void Hibernate()
    {
        Process.Start("shutdown", "/h");
    }
    
    public static void CancelShutdownCommand()
    {
        Process.Start("shutdown", "/a");
    }
}