using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Moq;
using Necati_Meral_Yahoo_De.Http;
using Necati_Meral_Yahoo_De.Logic.ComfortOnline;

namespace Necati_Meral_Yahoo_De.LogicNodes.Tests.ComfortOnline;
public class TestComfortOnlineRequestNode : ComfortOnlineRequestNode
{
    public TestComfortOnlineRequestNode(INodeContext context)
        : base(context)
    {
    }

    protected override IHttpClient CreateHttpClient()
    {
        var httpClientMock = new Mock<IHttpClient>();

        httpClientMock.Setup(c => c.GetStringAsync(It.Is<string>(x => x.EndsWith("/Account/Login") ) ))
            .Returns(() => GetEmbeddedResourceContentAsync("Login"));

        httpClientMock.Setup(c => c.GetStringAsync(It.Is<string>(x => x.Contains("/Measurand/Values"))))
            .Returns(() => GetEmbeddedResourceContentAsync("Measurand-Values"));

        httpClientMock.Setup(c => c.PostAsync(
            It.Is<string>(x => x.EndsWith("/Account/Login")),
            It.Is<NameValueCollection>(x =>
                x["UserName"] == ComfortOnlineTestConsts.UserName &&
                x["Password"] == ComfortOnlineTestConsts.Password &&
                x["__RequestVerificationToken"] == ComfortOnlineTestConsts.RequestVerificationToken
            )))
            .Returns(() => GetEmbeddedResourceContentAsync("Plant-List"));

        httpClientMock.Setup(c => c.PostAsync(
            It.Is<string>(x => x.EndsWith("/Account/Login")),
            It.Is<NameValueCollection>(x =>
                x["UserName"] != ComfortOnlineTestConsts.UserName ||
                x["Password"] != ComfortOnlineTestConsts.Password ||
                x["__RequestVerificationToken"] != ComfortOnlineTestConsts.RequestVerificationToken
            )))
            .Returns(() => GetEmbeddedResourceContentAsync("Login"));

        return httpClientMock.Object;
    }

    protected virtual async Task<string> GetEmbeddedResourceContentAsync(string file)
    {
        var fullFileName = $"{typeof(TestComfortOnlineRequestNode).Namespace}.TestData.{file}.html";
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fullFileName);
        using var reader = new StreamReader(stream);

        return await reader.ReadToEndAsync().ConfigureAwait(false);
    }
}
