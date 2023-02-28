using System.Collections.Generic;
using System.Threading.Tasks;

namespace Necati_Meral_Yahoo_De.Http;
public interface IHttpClient
{
    Task<string> GetStringAsync(string requestUri);
    Task<string> PostAsync(string requestUri, IDictionary<string, string> formData);
}
