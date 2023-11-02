using System.Diagnostics;

namespace RigControlApi.Utilities.PcControl;

public abstract class ApplicationCommands
{
    public static void CloseApplication(int applicationId)
    {
        var process = Process.GetProcessById(applicationId);
        process?.Kill();
    }
}