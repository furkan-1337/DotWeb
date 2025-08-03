using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace DotWeb;

public class Utilities
{
    public static List<IPAddress> GetLocalHostAddresses()
    {
        IPHostEntry host;
        host = Dns.GetHostEntry(Dns.GetHostName());
        List<IPAddress> ret = host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToList();

        return ret;
    }

    public static string GetWorkingDirectory()
    {
        string executablePath = Assembly.GetExecutingAssembly().Location;
        string executableDirectory = Path.GetDirectoryName(executablePath);
        return executableDirectory;
    }

    public static Dictionary<HttpStatusCode, string> ErrorPages = new Dictionary<HttpStatusCode, string>()
    {
        { HttpStatusCode.Forbidden, "/errors/403.html" },
        { HttpStatusCode.NotFound, "/errors/404.html" }
    };
    
    public static byte[] GetImage(string fullPath)
    {
        var filePath = $"{GetWorkingDirectory()}{Common.ServerDirectory}{fullPath}";
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
        var filePath = $"{GetWorkingDirectory()}{Common.ServerDirectory}{fullPath}";
        Common.Log.WriteLine(Log.LogLevel.Info, $"File path: {filePath}");
        if(File.Exists(filePath))
        {
            if(fullPath.StartsWith("/system"))
                return GetFile(ErrorPages[HttpStatusCode.Forbidden]);
            string text = File.ReadAllText(filePath);
            return Encoding.UTF8.GetBytes(text);
        }
        else
            return GetFile(ErrorPages[HttpStatusCode.NotFound]);
    }

    public static byte[] GetSource(string fullPath)
    {
        if(fullPath == "/" || fullPath == string.Empty)
            fullPath = "/index.html";
        return GetFile(fullPath);
    }
}