namespace Necati_Meral_Yahoo_De.Logic.Meltem;
public static class GetDeviceAction
{
    public const string GetVentilationPercent = "GetVentilationPercent";
    public const string GetVentilationLevelNumeric = "GetVentilationLevelNumeric";

    public static string[] Values { get; } = [
        GetVentilationPercent,
        GetVentilationLevelNumeric
    ];
}
