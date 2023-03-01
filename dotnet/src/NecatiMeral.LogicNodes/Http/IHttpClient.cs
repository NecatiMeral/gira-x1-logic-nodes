using System.Collections.Specialized;

namespace Necati_Meral_Yahoo_De.Http;
public interface IHttpClient
{
    Task<string> GetStringAsync(string requestUri);
    Task<string> PostAsync(string requestUri, NameValueCollection formData);
}
