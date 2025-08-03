using DotWeb.Utils;
using System.Net;

namespace DotWeb.Sandbox;

class Program
{
    private static DotWeb.Core.Server server = new DotWeb.Core.Server();
    static void Main(string[] args)
    {
        server
            .Add(Utilities.GetLocalHostAddresses())
            .Start();

        server.AddErrorHandler(HttpStatusCode.Forbidden, "/errors/403.html");
        server.AddErrorHandler(HttpStatusCode.NotFound, "/errors/403.html");
        
        server.AddHandler("GET", "/api/get_user", parameters =>
        {
            if(parameters.TryGetValue("username", out string username))
                return $"Input: {username}";
            else
                return "Wrong input.";
        });
        Console.ReadKey();
    }
}