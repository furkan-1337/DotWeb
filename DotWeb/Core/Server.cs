using System.Net;
using System.Text;

namespace DotWeb.Core;

public class Server
{
    private Router Router;
    private HttpListener Listener;

    private Thread thListen;
    private bool CanListen = false;
    public bool IsListening => Listener.IsListening;
    
    public Server()
    {
        Listener = new HttpListener();
        Listener.Prefixes.Add("http://localhost/");

        Router = new Router();
    }

    public void Start()
    {
        if(Listener.IsListening)
         throw new Exception("Listener is already listening.");
            
        Listener.Start();
        
        CanListen = true;
        thListen = new Thread(Listen);
        thListen.Start();
    }
    
    public void Stop()
    {
        if(!Listener.IsListening)
            throw new Exception("Listener is already not listening.");
        
        Listener.Stop();
        CanListen = false;
    }
    
    public async void Listen()
    {
        while (CanListen && Listener.IsListening)
        {
            HttpListenerContext context = await Listener.GetContextAsync();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            
            Common.Log.WriteLine(Log.LogLevel.Info, $"Received request: {request.HttpMethod} {request.Url}");
            Router.Route(context);
        }
        Common.Log.WriteLine(Log.LogLevel.Info, "Listener thread stopped.");
    }
    
    public Server Add(string ipAddress, int port = 0)
    {
        Listener.Prefixes.Add($"http://{ipAddress}{(port != 0 ? $":{port}" : string.Empty)}/");
        Common.Log.WriteLine(Log.LogLevel.Info,$"Listening on IP http://{ipAddress}{(port != 0 ? $":{port}" : string.Empty)}");
        return this;
    }

    public Server AddHandler(string method, string path, Func<Parameters, string> action)
    {
        Router.Handlers.Add((method, path, action));
        return this;
    }

    public Server AddErrorHandler(HttpStatusCode code, string path)
    {
        Router.ErrorHandlers.Add(code, path);
        return this;
    }
    
    public Server Add(List<IPAddress> ipAddresses, int port = 0)
    {
        ipAddresses.ForEach(ip =>
        {
            Add(ip.ToString(), port);
        });
        return this;
    }
}