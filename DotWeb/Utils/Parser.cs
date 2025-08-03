namespace DotWeb;

public class Parser
{
    public static (string Path, Parameters Params) ParseRawUrl(string rawUrl)
    {
        int qIndex = rawUrl.IndexOf('?');
        string path = qIndex >= 0 ? rawUrl.Substring(0, qIndex) : rawUrl;
        string rawParams = qIndex >= 0 ? rawUrl.Substring(qIndex + 1) : string.Empty;
        var parameters = ParseParams(rawParams);
        return (path, parameters);
    }
    
    private static Parameters ParseParams(string rawParameters)
    {
        var parameters = new Parameters();
        if(rawParameters.Contains("&"))
        {
            string[] pairs = rawParameters.Split('&');
            foreach (string pair in pairs)
            {
                var param = ParseParam(pair);
                if(IsValidParam(param))
                    parameters.Add(param.Key, param.Value);
            }
        }
        else
        {
            var param = ParseParam(rawParameters);
            if(IsValidParam(param))
                parameters.Add(param.Key, param.Value);
        }
        return parameters;
    }
    
    public static bool IsValidParam(Parameter parameter) => parameter.Key != null && parameter.Value != null;
    
    private static Parameter ParseParam(string rawParameter)
    {
        try
        {
            string[] pair = rawParameter.Split('=');
            return (pair[0], pair[1]);
        }
        catch (Exception e)
        {
            return (null, null);
        }
    }
}