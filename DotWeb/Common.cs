using DotWeb.Platform;
using System.Runtime.InteropServices;

namespace DotWeb;

public class Common
{
    public static string ServerDirectory = "/web";
    public static INativeWrapper Platform { get; private set; }
    public static Log Log { get; private set; } = new Log();
    
    static Common()
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            Platform = new Windows();
         else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
             throw new PlatformNotSupportedException("Linux platform is not supported yet.");
        else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            throw new PlatformNotSupportedException("macOS (OSX) platform is not supported yet.");
        else
            throw new PlatformNotSupportedException("Unsupported operating system detected.");
    }
}