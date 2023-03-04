using System.Net.Http;

namespace Necati_Meral_Yahoo_De;

public static class HttpMethodParser
{
    public static HttpMethod Parse(string method)
    {
        return method.ToUpperInvariant() switch
        {
            "GET" => HttpMethod.Get,
            "PUT" => HttpMethod.Put,
            "POST" => HttpMethod.Post,
            "DELETE" => HttpMethod.Delete,
            "HEAD" => HttpMethod.Head,
            _ => throw new NotImplementedException($"Method '{method}' is not implemented."),
        };
    }
}
