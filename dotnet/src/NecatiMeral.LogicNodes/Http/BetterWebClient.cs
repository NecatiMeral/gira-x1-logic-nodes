using System.Net;

namespace Necati_Meral_Yahoo_De.Http;
public class BetterWebClient : WebClient
{
    protected CookieContainer Cookies { get; } = new CookieContainer();

    protected override WebRequest GetWebRequest(Uri address)
    {
        var request = base.GetWebRequest(address);

        if (request is HttpWebRequest httpWebRequest)
        {
            httpWebRequest.CookieContainer = Cookies;
        }

        return request;
    }
}
