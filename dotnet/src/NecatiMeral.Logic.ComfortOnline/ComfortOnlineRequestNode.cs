using System.Collections.Specialized;
using System.Text.RegularExpressions;
using LogicModule.ObjectModel.TypeSystem;
using Necati_Meral_Yahoo_De.Helpers;
using Necati_Meral_Yahoo_De.Http;
using Necati_Meral_Yahoo_De.LogicNodes;

namespace Necati_Meral_Yahoo_De.Logic.ComfortOnline;
public class ComfortOnlineRequestNode : LocalizableNode
{
    protected ComfortOnlinePageParser Parser { get; }
    protected IHttpClient HttpClient { get; }

    [Input(DisplayOrder = 1, IsRequired = true)]
    public BoolValueObject Trigger { get; private set; }

    [Input(IsDefaultShown = true, DisplayOrder = 2)]
    public StringValueObject PlantId { get; }

    [Input(IsDefaultShown = true, DisplayOrder = 3)]
    public StringValueObject PlantSection { get; }

    [Input(IsDefaultShown = false, DisplayOrder = 4)]
    public StringValueObject UserName { get; }

    [Input(IsDefaultShown = false, DisplayOrder = 5)]
    public StringValueObject Password { get; }

    [Output(IsDefaultShown = true, DisplayOrder = 1)]
    public StringValueObject Data { get; private set; }

    [Output(IsDefaultShown = false, DisplayOrder = 99)]
    public StringValueObject Diagnostics { get; private set; }

    public ComfortOnlineRequestNode(INodeContext context)
        : base(context, nameof(ComfortOnlineRequestNode))
    {
        context.ThrowIfNull("context");

        Parser = new ComfortOnlinePageParser();

        Trigger = TypeService.CreateBool("BINARY", "Trigger", false);
        PlantId = TypeService.CreateString("STRING", "PlantId", string.Empty);
        PlantSection = TypeService.CreateString("STRING", "PlantSection", string.Empty);
        UserName = TypeService.CreateString("STRING", "UserName", string.Empty);
        Password = TypeService.CreateString("STRING", "Password", string.Empty);
        Data = TypeService.CreateString("STRING", "Data", string.Empty);
        Diagnostics = TypeService.CreateString("STRING", "Diagnostics", string.Empty);

        HttpClient = CreateHttpClient();
    }

    public override void Execute()
    {
        if (Trigger.WasSet && (bool)Trigger)
        {
            try
            {
                AsyncHelper.RunSync(ExecuteAsync);
            }
            catch (AggregateException ex)
            {
                Diagnostics.Value = $"{ComfortOnlineConsts.ErrorCodes.UnexpectedError} {ex.Message}";
                throw ex.InnerException;
            }
        }
    }

    protected virtual async Task ExecuteAsync()
    {
        var loginSucceeded = await LoginAsync();
        if (loginSucceeded)
        {
            var plantSectionResponse = await HttpClient.GetStringAsync($"/Measurand/Values?plant={PlantId.Value}&name={PlantSection.Value}");
            var parsed = Parser.Parse(plantSectionResponse);

            var entries = parsed.Values.Select(p => string.Format("\"val_{0}\": \"{1}\"", p.Key, string.Join(",", p.Value)));

            Data.Value = $"{{{string.Join(", ", entries)}}}";
            Diagnostics.Value = ComfortOnlineConsts.ErrorCodes.Ok;
        }
    }

    protected virtual async Task<bool> LoginAsync()
    {
        string initialRequestBody;
        try
        {
            initialRequestBody = await HttpClient.GetStringAsync("/Account/Login");
        }
        catch (Exception)
        {
            Diagnostics.Value = ComfortOnlineConsts.ErrorCodes.InitialRequestFailed;
            return false;
        }

        var requestVerificationToken = GetRequestVerificationToken(initialRequestBody);
        if (string.IsNullOrEmpty(requestVerificationToken))
        {
            Diagnostics.Value = ComfortOnlineConsts.ErrorCodes.MissingRequestVerificationToken;
            return false;
        }

        try
        {
            var loginPage = await HttpClient.PostAsync("/Account/Login", new NameValueCollection
            {
                { "UserName", UserName.Value },
                { "Password", Password.Value },
                { "__RequestVerificationToken", requestVerificationToken }
            });

            // Basic detection if we got redirected to login page = login failed
            if (loginPage.Contains("<form action=\"/account/login\""))
            {
                Diagnostics.Value = ComfortOnlineConsts.ErrorCodes.InvalidCredentials;
                return false;
            }
        }
        catch (Exception)
        {
            Diagnostics.Value = ComfortOnlineConsts.ErrorCodes.LoginFailed;
            return false;
        }

        return true;
    }

    protected virtual IHttpClient CreateHttpClient()
    {
        var httpClient = new BetterWebClient()
        {
            BaseAddress = ComfortOnlineConsts.ComfortOnlineBaseAddress
        };

        return new NetHttpClient(httpClient);
    }

    protected virtual string GetRequestVerificationToken(string body)
    {
        var matches = Regex.Match(body, "<input.*?(?:name=\\\"__RequestVerificationToken\\\".*?value=\\\"([^\"]+)|value=\\\"([^\"]+).*?name=\\\"__RequestVerificationToken\\\")[^>]*>");
        if (matches.Success)
        {
            return matches.Groups[1].Value;
        }
        return string.Empty;
    }
}
