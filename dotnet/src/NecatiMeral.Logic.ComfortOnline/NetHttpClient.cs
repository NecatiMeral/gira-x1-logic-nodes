using System.Net.Http;
using Necati_Meral_Yahoo_De.Http;

namespace Necati_Meral_Yahoo_De.Logic.ComfortOnline;
public class NetHttpClient : IHttpClient
{
    protected HttpClient HttpClient { get; }

    public NetHttpClient(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    public async Task<string> GetStringAsync(string requestUri)
    {
        return await HttpClient.GetStringAsync(requestUri).ConfigureAwait(false);
    }

    public async Task<string> PostAsync(string requestUri, IDictionary<string, string> formData)
    {
        var formContent = new FormUrlEncodedContent(formData);

        var response = await HttpClient.PostAsync("/Account/Login", formContent).ConfigureAwait(false);
        return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
    }
}
