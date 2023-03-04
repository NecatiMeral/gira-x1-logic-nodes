namespace Necati_Meral_Yahoo_De;

public static class GiraIOTUrlBuilder
{
    public static string GetUrl(string template, string hostOrIp, string clientToken, string dataPointUid)
    {
        return template.Replace("${HostOrIp}", hostOrIp, StringComparison.OrdinalIgnoreCase)
            .Replace("${ClientToken}", clientToken, StringComparison.OrdinalIgnoreCase)
            .Replace("${DataPointUid}", dataPointUid, StringComparison.OrdinalIgnoreCase);
    }
}
