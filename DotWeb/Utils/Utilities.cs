using DotWeb.Core;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace DotWeb.Utils;

public class Utilities
{
    public static List<IPAddress> GetLocalHostAddresses()
    {
        IPHostEntry host;
        host = Dns.GetHostEntry(Dns.GetHostName());
        List<IPAddress> ret = host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToList();

        return ret;
    }

    public static string GetExecutableDirectory()
    {
        string executablePath = Assembly.GetExecutingAssembly().Location;
        string executableDirectory = Path.GetDirectoryName(executablePath);
        return executableDirectory;
    }
    
    public static byte[] GetImage(string fullPath)
    {
        var filePath = $"{GetExecutableDirectory()}{Common.ServerDirectory}{fullPath}";
        Common.Log.WriteLine(Log.LogLevel.Info, $"File path: {filePath}");
        if(File.Exists(filePath))
        {
            FileStream fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fStream);
            try
            {
                return br.ReadBytes((int)fStream.Length);
            }
            finally
            {
                br.Close();
                fStream.Close();
            }
        }
        else 
            return Array.Empty<byte>();
    }

    public static byte[] GetFile(string fullPath)
    {
        var filePath = $"{GetExecutableDirectory()}{Common.ServerDirectory}{fullPath}";
        Common.Log.WriteLine(Log.LogLevel.Info, $"File path: {filePath}");
        if(File.Exists(filePath))
        {
            if(fullPath.StartsWith("/system"))
                return GetFile(Router.ErrorHandlers[HttpStatusCode.Forbidden]);
            string text = File.ReadAllText(filePath);
            return Encoding.UTF8.GetBytes(text);
        }
        else
            return GetFile(Router.ErrorHandlers[HttpStatusCode.NotFound]);
    }

    public static byte[] GetSource(string fullPath)
    {
        if(fullPath == "/" || fullPath == string.Empty)
            fullPath = Common.DefaultPage;
        return GetFile(fullPath);
    }
}