using System.Runtime.InteropServices;

namespace RigControlApi.Utilities;

public class MemoryInformation
{
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr hObject);

    [StructLayout(LayoutKind.Sequential)]
    private class ProcessMemoryCounters
    {
        public uint cb;
        public uint PageFaultCount;
        public UIntPtr PeakWorkingSetSize;
        public UIntPtr WorkingSetSize;
        public UIntPtr QuotaPeakPagedPoolUsage;
        public UIntPtr QuotaPagedPoolUsage;
        public UIntPtr QuotaPeakNonPagedPoolUsage;
        public UIntPtr QuotaNonPagedPoolUsage;
        public UIntPtr PagefileUsage;
        public UIntPtr PeakPagefileUsage;
    }

    [DllImport("psapi.dll", SetLastError = true)]
    private static extern bool GetProcessMemoryInfo(IntPtr hProcess, [Out] ProcessMemoryCounters counters, uint size);

    public static long GetMemoryUsageForProcess(long pid)
    {
        long mem;
        var pHandle = OpenProcess(0x0400 | 0x0010, false, (uint)pid);
        if (pHandle == IntPtr.Zero)
        {
            throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
        }

        try
        {
            var pmc = new ProcessMemoryCounters { cb = (uint)Marshal.SizeOf(typeof(ProcessMemoryCounters)) };
            if (GetProcessMemoryInfo(pHandle, pmc, pmc.cb))
            {
                mem = (long)pmc.WorkingSetSize;
            }
            else
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
        }
        finally
        {
            CloseHandle(pHandle);
        }
        return mem;
    }
}
