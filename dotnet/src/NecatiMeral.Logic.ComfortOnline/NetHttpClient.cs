using System.Collections.Specialized;
using System.Net;
using System.Text;
using Necati_Meral_Yahoo_De.Http;

namespace Necati_Meral_Yahoo_De.Logic.ComfortOnline;
public class NetHttpClient : IHttpClient
{
    protected WebClient HttpClient { get; }

    public NetHttpClient(WebClient httpClient)
    {
        HttpClient = httpClient;
    }

    public async Task<string> GetStringAsync(string requestUri)
    {
        return await HttpClient.DownloadStringTaskAsync(requestUri).ConfigureAwait(false);
    }

    public async Task<string> PostAsync(string requestUri, NameValueCollection formData)
    {
        //var parameters =  GetParametersData(formData);

        var body = await HttpClient.UploadValuesTaskAsync("/Account/Login", formData).ConfigureAwait(false);
        return Encoding.UTF8.GetString(body);

    }

    protected string GetParametersData(IDictionary<string, string> formData)
    {
        var sb = new StringBuilder();

        foreach (var key in formData.Keys)
        {
            sb.AppendFormat("{0}={1}&", key, formData[key]);
        }
        return sb.ToString().Substring(0, sb.Length -1);
    }
}
