using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace Necati_Meral_Yahoo_De.LogicNodes.Tests.Core;
public static class PortManager
{
    private static object _syncRoot = new();
    private static readonly HashSet<int> _reservedPorts = [];

    /// <summary>
    /// checks for used ports and retrieves the first free port
    /// </summary>
    /// <returns>the free port or 0 if it did not find a free port</returns>
    public static int GetAvailablePort(int startingPort)
    {
        lock (_syncRoot)
        {
            var port = GetAvailablePortCore(startingPort);
            _reservedPorts.Add(port);
            return port;
        }
    }

    /// <summary>
    /// checks for used ports and retrieves the first free port
    /// </summary>
    /// <returns>the free port or 0 if it did not find a free port</returns>
    private static int GetAvailablePortCore(int startingPort)
    {
        IPEndPoint[] endPoints;
        var portArray = new List<int>(_reservedPorts);

        var properties = IPGlobalProperties.GetIPGlobalProperties();

        // getting active connections
        var connections = properties.GetActiveTcpConnections();
        portArray.AddRange(from n in connections
                           where n.LocalEndPoint.Port >= startingPort
                           select n.LocalEndPoint.Port);

        // getting active tcp listners - WCF service listening in tcp
        endPoints = properties.GetActiveTcpListeners();
        portArray.AddRange(from n in endPoints
                           where n.Port >= startingPort
                           select n.Port);

        // getting active udp listeners
        endPoints = properties.GetActiveUdpListeners();
        portArray.AddRange(from n in endPoints
                           where n.Port >= startingPort
                           select n.Port);

        portArray.Sort();

        for (var i = startingPort; i < ushort.MaxValue; i++)
            if (!portArray.Contains(i))
                return i;

        return 0;
    }
}
