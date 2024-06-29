namespace Necati_Meral_Yahoo_De.Logic.Meltem;
public static class VentilationPreset
{
    public const string Off = "Off";
    public const string ReducedVentilation = "ReducedVentilation";
    public const string NominalVentilation = "NominalVentilation";
    public const string IncreasedVentilation1 = "IncreasedVentilation1";
    public const string IncreasedVentilation2 = "IncreasedVentilation2";
    public const string IntensiveVentilation = "IntensiveVentilation";

    public static string[] Values { get; } = [
        Off,
        ReducedVentilation,
        NominalVentilation,
        IncreasedVentilation1,
        IncreasedVentilation2,
        IntensiveVentilation
    ];

    public static IDictionary<int, int> PresetMap { get; } = new Dictionary<int, int>
    {
        { 0, 0 },
        { 1, 10 },
        { 2, 30 },
        { 3, 50 },
        { 4, 70 },
        { 5, 100 }
    };

    public static int GetVentilationPercent(string preset)
        => GetVentilationPercent(Array.FindIndex(Values, x => x == preset));

    public static int GetVentilationPercent(int level)
    {
        // Fallback to nominal ventilation
        return PresetMap.TryGetValue(level, out var percent)
            ? percent
            : 30;
    }

    public static int GetNearestVentilationLevel(int percent)
    {
        var level = PresetMap.FirstOrDefault(x => x.Value >= percent).Key;
        return level;
    }
}
