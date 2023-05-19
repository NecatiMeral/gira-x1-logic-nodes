namespace Necati_Meral_Yahoo_De.Logic.Ads;
public static class AdsType
{
    public const string Int16 = "16 bit integer";

    public const string String = "String";

    public const string Boolean = "Boolean";

    public static string[] Values { get; } = new[] { Int16, String, Boolean };
}
