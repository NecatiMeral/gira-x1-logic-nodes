using System.Net.Http;
using Microsoft.Extensions.Options;
using Necati_Meral_Yahoo_De;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DoorbirdOptions>(builder.Configuration);

// NM@04.03.2023: Configure http-client for X1 api
// The HTTPS connection is not fully trusted, because it is not technically possible
// that the server provides a trusted TLS certificate. Because of this, the client that is used to
// access the Gira IoT REST API needs to skip the certification check. 
builder.Services.AddHttpClient(DoorbirdX1ListenerConsts.HttpClientName)
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        return new HttpClientHandler
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        };
    });

var app = builder.Build();

app.MapGet("/trigger/{key}/{clientToken}/{dataPointUid}", async (
    string key,
    string clientToken,
    string dataPointUid,
    string target,
    IHttpClientFactory httpClientFactory,
    IOptions<DoorbirdOptions> options) =>
{
    var integration = options.Value.Doorbirds.GetOrDefault(key);

    var hostOrIp = string.IsNullOrEmpty(integration.HostOrIp) ? target : integration.HostOrIp;
    if (string.IsNullOrEmpty(hostOrIp))
    {
        return Results.NotFound();
    }

    var httpClient = httpClientFactory.CreateClient(DoorbirdX1ListenerConsts.HttpClientName);
    var method = HttpMethodParser.Parse(integration.Method);
    var url = GiraIOTUrlBuilder.GetUrl(integration.UrlTemplate, hostOrIp, clientToken, dataPointUid);

    var response = await httpClient
        .SendAsync(new HttpRequestMessage(method, url) { Content = new StringContent(integration.Payload)})
        .ConfigureAwait(false);

    // NM@04.03.2023: Return service status-code to caller
    var content = await response.Content.ReadAsStringAsync()
        .ConfigureAwait(false);

    return Results.StatusCode((int)response.StatusCode);
});

await app.RunAsync();
