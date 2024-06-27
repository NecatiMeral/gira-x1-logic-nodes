namespace Necati_Meral_Yahoo_De.Logic.Meltem;
public static class SetDeviceAction
{
    public const string SetBalancedVentilationPercent = "SetBalancedVentilationPercent";

    public const string SetUnbalancedVentilationPercent = "SetUnbalancedVentilationPercent";

    public static string[] Values { get; } = [
        SetBalancedVentilationPercent,
        SetUnbalancedVentilationPercent
    ];
}
