using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using FluentModbus;
using Necati_Meral_Yahoo_De.Logic.Meltem;
using Necati_Meral_Yahoo_De.LogicNodes.Tests;
using Shouldly;
using Xunit.Abstractions;

namespace NecatiMeral.LogicNodes.Meltem.Tests;
public class GetVentilationNodeTests : NodeTestBase<GetVentilationNode>
{
    private readonly int _port;
    private readonly ModbusTcpServer _server;

    protected int UnitIdentifier { get; } = 5;

    public GetVentilationNodeTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
        _port = GetAvailablePort(500);
        _server = new ModbusTcpServer(Logger);
        _server.AddUnit((byte)UnitIdentifier);
        _server.Start(new IPEndPoint(IPAddress.Any, _port));

        ConfigureMeltemNode(Sut);
    }

    private void ConfigureMeltemNode(MeltemNodeBase node)
    {
        node.UnitId.Value = UnitIdentifier;
        node.IPAddress.Value = "localhost";
        node.Port.Value = _port;
    }

    [Fact]
    public async Task Should_Get_Balanced_Ventilation()
    {
        // Arrange
        SetVentilation(100);

        await Task.Delay(TimeSpan.FromSeconds(3));

        Sut.Action.Value = GetDeviceAction.GetVentilationPercent;
        Sut.Trigger.Value = true;

        // Act
        Sut.Execute();

        // Assert
        Sut.VentilationPercentage.Value.ShouldBe(100);
    }

    [Fact]
    public async Task Should_Get_Unbalanced_Ventilation()
    {
        // Arrange
        SetVentilation(100, 0);

        await Task.Delay(TimeSpan.FromSeconds(3));

        Sut.Action.Value = GetDeviceAction.GetVentilationPercent;
        Sut.Trigger.Value = true;

        // Act
        Sut.Execute();

        // Assert
        Sut.VentilationPercentage.Value.ShouldBe(50);
    }

    void SetVentilation(int balancedOrIntake, int? exhaust = null)
    {
        var node = CreateNode<SetVentilationNode>();
        ConfigureMeltemNode(node);

        if (exhaust.HasValue)
        {
            node.Action.Value = SetDeviceAction.SetUnbalancedVentilationPercent;
            node.UnbalancedIntakeVentilation.Value = balancedOrIntake;
            node.UnbalancedExhaustVentilation.Value = exhaust.Value;
        }
        else
        {
            node.Action.Value = SetDeviceAction.SetBalancedVentilationPercent;
            node.BalancedVentilation.Value = balancedOrIntake;
        }

        node.Execute();
    }

    /// <summary>
    /// checks for used ports and retrieves the first free port
    /// </summary>
    /// <returns>the free port or 0 if it did not find a free port</returns>
    public static int GetAvailablePort(int startingPort)
    {
        IPEndPoint[] endPoints;
        var portArray = new List<int>();

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

        for (var i = startingPort; i < UInt16.MaxValue; i++)
            if (!portArray.Contains(i))
                return i;

        return 0;
    }
}
