using System;

namespace Necati_Meral_Yahoo_De.LogicNodes.Helpers;
public static class EnvironmentHelper
{
    public static bool IsSimulation()
    {
        return Environment.OSVersion.VersionString.ToUpperInvariant().Contains("WIN");
    }
}
