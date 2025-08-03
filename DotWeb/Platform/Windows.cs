using System.Runtime.InteropServices;

namespace DotWeb.Platform;

public class Windows : INativeWrapper
{
    public bool CreateConsole()
    {
        return AllocConsole();
    }
    
    public bool DestoryConsole()
    {
        return FreeConsole();
    }
    
    public bool HasConsole()
    {
        return GetConsoleWindow() != nint.Zero;
    }
    
    // for Dynamic Import - WIP
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr LoadLibrary(string dllToLoad);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);
    
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GetConsoleWindow();

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool AllocConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool FreeConsole();
}