using System.Text;

namespace DotWeb;

public class Log
{
    private StringBuilder _log = new StringBuilder();
    
    public void Write(string message)
    {
        if(Common.Platform.HasConsole())
            Console.Write(message);
        
        lock (_log)
            _log.Append(message);
    }

    public void WriteLine(string message)
    {
        Write($"{message}{Environment.NewLine}");
    }

    public void WriteLine(LogLevel level, string message)
    {
        WriteLine($"[{level}] {message}");
    }
    
    public string GetContent()
    {
        lock (_log)
            return _log.ToString();
    }
    
    public enum LogLevel
    {
        Trace,
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }
}