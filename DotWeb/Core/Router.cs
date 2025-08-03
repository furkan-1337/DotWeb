using DotWeb.Core.Structs;
using System.Net;
using System.Text;
namespace DotWeb.Core;

public class Router
{
    public async void Route(HttpListenerContext context)
    {
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;

        var urlInfo = Parser.ParseRawUrl(request.RawUrl);
        string paramLog = urlInfo.Params.Count > 0
            ? string.Join(", ", urlInfo.Params.Select(p => $"{p.Key}={p.Value}"))
            : string.Empty;
        Common.Log.WriteLine(Log.LogLevel.Info, $"Request Method: {request.HttpMethod} Path: {urlInfo.Path} Parameters: {paramLog}");
        foreach (var handler in Handlers)
        {
            if(handler.Method == request.HttpMethod && handler.Path == urlInfo.Path)
            {
                string result = handler.action(urlInfo.Params);
                byte[] buffer = Encoding.UTF8.GetBytes(result);
                response.ContentType = "text/plain";
                response.StatusCode = 200;
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                response.OutputStream.Close();
                return;
            }
        }
        
        Common.Log.WriteLine(Log.LogLevel.Info, 
                $"Request Method: {request.HttpMethod}, Path: {urlInfo.Path}, Encoding: {Encoding.UTF8.WebName}");
        Packet packet = new Packet(request.HttpMethod, urlInfo.Path, Encoding.UTF8);
        
        await response.OutputStream.WriteAsync(packet.Data, 0, packet.Data.Length);
        response.OutputStream.Close();
    }

    public List<(string Method, string Path, Func<Parameters, string> action)> Handlers = new List<(string Method, string Path, Func<Parameters, string> action)>();
}