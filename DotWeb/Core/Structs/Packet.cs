using DotWeb.Core.Enums;
using DotWeb.Utils;
using System.Text;

namespace DotWeb.Core.Structs;

public struct Packet
{
    public static Dictionary<string, string> ContentTypes = new Dictionary<string, string>()
    {
        {".ico", "image/ico"},
        {".png", "image/png"},
        {".jpg", "image/jpg"},
        {".gif", "image/gif"},
        {".bmp", "image/bmp"},
        {".html", "text/html"},
        {".css", "text/css"},
        {".js", "text/javascript"},
        {".txt", "text/plain"},
        {string.Empty, "text/html"},
    };
    
    public string Method;
    public string Path;
    public byte[] Data;
    public FileType Type;
    public Encoding Encoding;
    public string ContentType;
    public long ContentLength => Data.Length;
    
    public Packet(string method, string path, Encoding encoding)
    {
        Method = method;
        Path = path;
        Encoding = encoding;
        string extension = System.IO.Path.GetExtension(path);
        if(ContentTypes.TryGetValue(extension, out string contentType))
        {
            if(contentType == "text/html" || contentType == string.Empty)
                Type = FileType.Source;
            else if(contentType.StartsWith("text"))
                Type = FileType.File;
            else if(contentType.StartsWith("image"))
                Type = FileType.Image;
        }
        else
            throw new NotSupportedException("Not supported content type.");
        
        if(Type == FileType.File)
            Data = Utilities.GetFile(path);
        else if(Type == FileType.Source)
            Data = Utilities.GetSource(path);
        else if(Type == FileType.Image)
            Data = Utilities.GetImage(path);
        ContentType = contentType;
    }
}