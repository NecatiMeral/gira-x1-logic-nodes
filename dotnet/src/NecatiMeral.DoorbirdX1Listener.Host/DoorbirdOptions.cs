namespace Necati_Meral_Yahoo_De;

public class DoorbirdOptions
{
    public DoorbirdConfigurationDictionary Doorbirds { get; set; }

    public DoorbirdOptions()
    {
        Doorbirds = new DoorbirdConfigurationDictionary();
    }
}

public class DoorbirdConfigurationDictionary : Dictionary<string, DoorbirdConfiguration>
{
    public const string DefaultName = "Default";

    public DoorbirdConfiguration Default
    {
        get => TryGetValue(DefaultName, out var config) ? config : null;
        set => this[DefaultName] = value;
    }

    public DoorbirdConfiguration GetOrDefault(string key)
    {
        return TryGetValue(key, out var config) ? config : Default;
    }
}

public class DoorbirdConfiguration
{
    public string UrlTemplate { get; set; } = "https://${HostOrIp}/api/v2/values/${DataPointUid}?token=${ClientToken}";
    public string HostOrIp { get; set; }
    public string Method { get; set; } = "PUT";
    public string Payload { get; set; } = "{ \"value\": \"1\" }";
}
